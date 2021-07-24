using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using KeyboardTrainer.Core;
using KeyboardTrainer.Model;
using System;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace KeyboardTrainer.ViewModel
{
	public class HomeViewModel : ViewModelBase
	{
		public HomeViewModel()
		{
			_errorsFraction = new Fraction();
			_timer = new DispatcherTimer();
			_timer.Interval = new TimeSpan(0, 0, 1);
			_timer.Tick += OnTick;
			InputLanguageManager.Current.InputLanguageChanged += LanguageChanged;
			_typedText = new StringBuilder(17);
			_nextText = new StringBuilder();
			_stopwatch = new Stopwatch();

			LoadedCommand = new RelayCommand(Reset);
			SaveResultCommand = new RelayCommand(SaveResult);
		}

		#region Properties
		public RelayCommand<TextCompositionEventArgs> TextInputCommand { get; }
		public RelayCommand LoadedCommand { get; }
		public RelayCommand SaveResultCommand { get; }

		public string NextText
		{
			get => _nextText.ToString();
			private set
			{
				_nextText.Clear();
				_nextText.Append(value);

				RaisePropertyChanged(nameof(NextText));
			}
		}

		public string TypedText
		{
			get => _typedText.ToString();
			private set
			{
				_typedText.Clear();
				_typedText.Append(value);
				RaisePropertyChanged(nameof(TypedText));
			}
		}

		public TimeSpan Time
		{
			get => _time;
			private set
			{
				_time = value;
				RaisePropertyChanged(nameof(Time));
			}
		}

		public Visibility ResultVisibility
		{
			get => _resultVisibility;
			private set
			{
				_resultVisibility = value;
				RaisePropertyChanged(nameof(ResultVisibility));
			}
		}

		public bool IsUpperKeys
		{
			get => _isUpperKeys;
			set
			{
				_isUpperKeys = value;
				RaisePropertyChanged(nameof(IsUpperKeys));
			}
		}

		public char RequiredKey
		{
			get => _requiredKey;
			private set
			{
				_requiredKey = value;
				RaisePropertyChanged(nameof(RequiredKey));
			}
		}

		public int CharPerMinute => Time.TotalMinutes == 0 ?
			0 :
			(int)Math.Round(_typedKey / Time.TotalMinutes);

		public double ErrorsPercent => _errorsFraction.GetPercent();
		#endregion

		private readonly StringBuilder _typedText;
		private readonly StringBuilder _nextText;
		private string _currentVocabulary;
		private int _typedKey = 0;
		private int _errorsCount = 0;
		private Fraction _errorsFraction;
		private readonly DispatcherTimer _timer;
		private TimeSpan _time = new TimeSpan(1);
		private readonly Stopwatch _stopwatch;
		private Visibility _resultVisibility = Visibility.Hidden;
		private bool _isUpperKeys;
		private char _requiredKey;

		public void Reset()
		{
			_errorsCount = 0;
			_stopwatch.Reset();
			_timer.Stop();
			SetErrorsFraction(new Fraction());
			SetTypedKey(0);

			Time = TimeSpan.Zero;
			TypedText = "";
			NextText = Vocabularies.Instance.GetContent(5, true);
			RequiredKey = NextText[0];
			_currentVocabulary = Vocabularies.Instance.Current.Name;
		}

		private void OnTick(object sender, EventArgs e)
		{
			Time = _stopwatch.Elapsed;
			RaisePropertyChanged(nameof(CharPerMinute));
		}

		private void LanguageChanged(object sender, InputLanguageEventArgs e)
		{
			RaisePropertyChanged(nameof(IsUpperKeys));
			RaisePropertyChanged(nameof(RequiredKey));
		}

		public void TextInput(TextCompositionEventArgs args)
		{
			var key = args.Text;

			if (key == "\b" || NextText.Length == 0)
			{
				return;
			}
			var expectedKey = NextText[0].ToString();

			if (!_timer.IsEnabled)
			{
				_stopwatch.Start();
				_timer.Start();
			}

			if (key != expectedKey)
			{
				_errorsCount++;
				SetErrorsFraction(new Fraction(_errorsCount, _typedKey + _errorsCount));
				return;
			}

			SetTypedKey(_typedKey + 1);
			SetErrorsFraction(new Fraction(_errorsCount, _typedKey + _errorsCount));
			ShiftNextText();
			AddTypedText(key);

			if (NextText.Length == 0)
			{
				TypingEnd();
			}
			else
			{
				RequiredKey = NextText[0];
			}
		}

		private void TypingEnd()
		{
			_timer.Stop();
			_stopwatch.Stop();
			Time = _stopwatch.Elapsed;
			ResultVisibility = Visibility.Visible;
		}

		private void SaveResult()
		{
			var statistic = new Statistic();
			statistic.Add(
				new Result(
					_currentVocabulary,
					CharPerMinute,
					ErrorsPercent,
					Time.Ticks,
					DateTimeOffset.Now));

			statistic.Save();

			Reset();
			ResultVisibility = Visibility.Hidden;
		}

		private void SetErrorsFraction(Fraction value)
		{
			_errorsFraction = value;
			RaisePropertyChanged(nameof(ErrorsPercent));
		}

		private void SetTypedKey(int value)
		{
			_typedKey = value;
			RaisePropertyChanged(nameof(CharPerMinute));
		}

		private void AddTypedText(string letter)
		{
			if (_typedText.Length > 16)
			{
				_typedText.Remove(0, 1);
			}
			_typedText.Append(letter);
			RaisePropertyChanged(nameof(TypedText));
		}

		private void ShiftNextText()
		{
			_nextText.Remove(0, 1);
			RaisePropertyChanged(nameof(NextText));
		}
	}
}

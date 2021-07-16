using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using KeyboardTrainer.Model;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace KeyboardTrainer.ViewModel
{
	public class HomeViewModel : ViewModelBase
	{
		#region properties

		public RelayCommand<TextCompositionEventArgs> TextInputCommand { get; set; }
		public RelayCommand LoadedCommand { get; set; }
		public RelayCommand SaveResultCommand { get; set; }

		public string NextText
		{
			get => _nextText.ToString();
			set
			{
				_nextText.Clear();
				_nextText.Append(value);
				RaisePropertyChanged(nameof(NextText));
			}
		}

		public string TypedText
		{
			get => _typedText.ToString();
			set
			{
				_typedText.Clear();
				_typedText.Append(value);
				RaisePropertyChanged(nameof(TypedText));
			}
		}

		public int TypedKey
		{
			get => _typedKey;
			set
			{
				_typedKey = value;
				RaisePropertyChanged(nameof(CharPerMinute));
			}
		}

		public int CharPerMinute =>
			Time.TotalMinutes == 0 ? 0 :
			(int)Math.Round(_typedKey / Time.TotalMinutes);

		public Fraction ErrorsFraction
		{
			get => _errorsFraction;
			set
			{
				_errorsFraction = value;
				RaisePropertyChanged(nameof(ErrorsPercent));
			}
		}

		public double ErrorsPercent => ErrorsFraction.GetPercent();

		public TimeSpan Time
		{
			get => _time;
			set
			{
				_time = value;
				RaisePropertyChanged(nameof(Time));
			}
		}

		public Visibility ResultVisibility
		{
			get => _resultVisibility;
			set
			{
				_resultVisibility = value;
				RaisePropertyChanged(nameof(ResultVisibility));
			}
		}

		#endregion

		private readonly StringBuilder _typedText;
		private readonly StringBuilder _nextText;
		private int _typedKey = 0;
		private int _errorsCount = 0;
		private Fraction _errorsFraction;
		private readonly DispatcherTimer _timer;
		private TimeSpan _time = new TimeSpan(1);
		private readonly Stopwatch _stopwatch;
		private Visibility _resultVisibility = Visibility.Hidden;

		public HomeViewModel()
		{
			_errorsFraction = new Fraction();
			_timer = new DispatcherTimer();
			_timer.Interval = new TimeSpan(0, 0, 1);
			_timer.Tick += OnTick;

			_typedText = new StringBuilder();
			_nextText = new StringBuilder();
			_stopwatch = new Stopwatch();
			Vocabularies.GetInstance();

			TextInputCommand = new RelayCommand<TextCompositionEventArgs>(TextInput);
			LoadedCommand = new RelayCommand(Load);
			SaveResultCommand = new RelayCommand(SaveResult);
		}

		private void OnTick(object sender, EventArgs e)
		{
			Time = _stopwatch.Elapsed;
			RaisePropertyChanged(nameof(CharPerMinute));
		}

		private void TextInput(TextCompositionEventArgs args)
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
				ErrorsFraction = new Fraction(_errorsCount, _typedKey + _errorsCount);
				return;
			}

			TypedKey++;
			ErrorsFraction = new Fraction(_errorsCount, _typedKey + _errorsCount);
			ShiftNextText();
			AddTypedText(key);

			if (NextText.Length == 0)
			{
				TypingEnd();
			}
		}

		private void Load()
		{
			var vocabularies = Vocabularies.GetInstance();
			_errorsCount = 0;
			_stopwatch.Reset();
			ErrorsFraction = new Fraction();
			TypedKey = 0;

			Time = TimeSpan.Zero;
			TypedText = "";
			NextText = vocabularies.GetContent(5, true);
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
			statistic.Add(new Result(CharPerMinute, ErrorsPercent, Time, DateTimeOffset.Now));
			statistic.Save();

			Load();
			ResultVisibility = Visibility.Hidden;
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

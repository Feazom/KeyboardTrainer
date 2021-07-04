using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace KeyboardTrainer.ViewModel
{
	public class HomeViewModel : ViewModelBase
	{
		public RelayCommand<KeyEventArgs> KeyDownCommand { get; set; }
		public RelayCommand<TextChangedEventArgs> TextChangedCommand { get; set; }

		private string _text;

		public string Text
		{
			get => _text;
			set
			{
				_text = value;
				RaisePropertyChanged("Text");
			}
		}

		public HomeViewModel()
		{
			KeyDownCommand = new RelayCommand<KeyEventArgs>(KeyDown);
			TextChangedCommand = new RelayCommand<TextChangedEventArgs>(TextChanged);
		}

		private void TextChanged(TextChangedEventArgs args)
		{

		}

		private void KeyDown(KeyEventArgs args)
		{
			var ch = (char)KeyInterop.VirtualKeyFromKey(args.Key);
			if (!char.IsLetter(ch)) return;

			ch = KeyToChar(ch);

			var box = args.Source as TextBox;
			var text = box.Text;
			var caret = box.CaretIndex;

			var local = ch.ToString();

			box.Text = text.Insert(caret, local);
			box.CaretIndex = caret + local.Length;
			args.Handled = true;
		}

		private char KeyToChar(char ch)
		{
			if (!char.IsLetter(ch))
			{
				throw new ArgumentException();
			}

			bool isUpper = false;
			if (Keyboard.IsKeyToggled(Key.Capital) || Keyboard.IsKeyToggled(Key.CapsLock))
			{
				isUpper = !isUpper;
			}
			if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
			{
				isUpper = !isUpper;
			}

			return !isUpper ? char.ToLower(ch) : ch;
		}
	}
}

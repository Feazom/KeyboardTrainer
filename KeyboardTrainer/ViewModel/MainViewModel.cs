using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using KeyboardTrainer.Core.Audio;
using KeyboardTrainer.Model;
using System.Windows;
using System.Windows.Input;

namespace KeyboardTrainer.ViewModel
{
	public class MainViewModel : ViewModelBase
	{
		public MainViewModel()
		{
			_ = Vocabularies.Instance;
			_homeVm = ViewModelLocator.GetViewModel<HomeViewModel>();

			CurrentView = ViewModelLocator.GetViewModel<HomeViewModel>();

			HomeViewCommand = new RelayCommand(ChangeViewTo<HomeViewModel>);
			SettingsViewCommand = new RelayCommand(ChangeViewTo<SettingsViewModel>);
			StatisticViewCommand = new RelayCommand(ChangeViewTo<StatisticViewModel>);
			CloseWindowCommand = new RelayCommand<Window>(Close);
			MinimizeWindowCommand = new RelayCommand<Window>(MinimizeWindow);
			KeyDownCommand = new RelayCommand<KeyEventArgs>(KeyDown);
			KeyUpCommand = new RelayCommand<KeyEventArgs>(KeyUp);
			TextInputCommand = new RelayCommand<TextCompositionEventArgs>(TextInput);
			ClosingCommand = new RelayCommand(Closing);
		}

		#region Properties
		public RelayCommand HomeViewCommand { get; }
		public RelayCommand SettingsViewCommand { get; }
		public RelayCommand StatisticViewCommand { get; }
		public RelayCommand<Window> CloseWindowCommand { get; }
		public RelayCommand<Window> MinimizeWindowCommand { get; }
		public RelayCommand<KeyEventArgs> KeyDownCommand { get; }
		public RelayCommand<KeyEventArgs> KeyUpCommand { get; }
		public RelayCommand<TextCompositionEventArgs> TextInputCommand { get; }
		public RelayCommand ClosingCommand { get; }

		public object CurrentView
		{
			get => _currentView;
			private set
			{
				if (_currentView == value)
				{
					return;
				}

				_currentView = value;
				RaisePropertyChanged(nameof(CurrentView));
			}
		}
		#endregion

		private object _currentView;
		private readonly HomeViewModel _homeVm;

		private void Closing()
		{
			AudioPlaybackEngine.Instance.Dispose();
			ViewModelLocator.GetViewModel<SettingsViewModel>().SaveSetting();
		}

		private void KeyDown(KeyEventArgs args)
		{
			if (args.Key == Key.LeftCtrl || args.Key == Key.RightCtrl)
			{
				return;
			}
			var capsToggled = Keyboard.GetKeyStates(Key.CapsLock) == KeyStates.Toggled ||
				Keyboard.GetKeyStates(Key.CapsLock) == (KeyStates.Toggled | KeyStates.Down);
			var shiftToggled = args.KeyboardDevice.Modifiers == ModifierKeys.Shift;

			_homeVm.IsUpperKeys = capsToggled ^ shiftToggled;
		}

		private void TextInput(TextCompositionEventArgs args)
		{
			if (CurrentView is HomeViewModel)
			{
				ViewModelLocator.GetViewModel<HomeViewModel>().TextInput(args);
			}
		}

		private void KeyUp(KeyEventArgs args)
		{
			if (args.Key == Key.LeftCtrl || args.Key == Key.RightCtrl)
			{
				return;
			}
			var capsToggled = Keyboard.GetKeyStates(Key.CapsLock) == KeyStates.Toggled;
			var shiftToggled = args.KeyboardDevice.Modifiers == ModifierKeys.Shift;

			_homeVm.IsUpperKeys = capsToggled ^ shiftToggled;
		}

		private void ChangeViewTo<T>() where T : ViewModelBase
		{
			CurrentView = ViewModelLocator.GetViewModel<T>();
		}

		private void Close(Window window)
		{
			window.Close();
		}

		private void MinimizeWindow(Window window)
		{
			window.WindowState = WindowState.Minimized;
		}
	}
}
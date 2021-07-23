using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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
			MoveWindowCommand = new RelayCommand<Window>(MoveWindow);
			KeyDownCommand = new RelayCommand<KeyEventArgs>(KeyDown);
			KeyUpCommand = new RelayCommand<KeyEventArgs>(KeyUp);
		}

		#region Properties
		public RelayCommand HomeViewCommand { get; }
		public RelayCommand SettingsViewCommand { get; }
		public RelayCommand StatisticViewCommand { get; }
		public RelayCommand<Window> CloseWindowCommand { get; }
		public RelayCommand<Window> MinimizeWindowCommand { get; }
		public RelayCommand<Window> MoveWindowCommand { get; }
		public RelayCommand<KeyEventArgs> KeyDownCommand { get; }
		public RelayCommand<KeyEventArgs> KeyUpCommand { get; }

		public object CurrentView
		{
			get => _currentView;
			set
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

		private void KeyDown(KeyEventArgs args)
		{
			if (args.Key == Key.LeftCtrl || args.Key == Key.RightCtrl)
			{
				return;
			}
			var capsToggled = Keyboard.GetKeyStates(Key.CapsLock) == KeyStates.Toggled ||
				Keyboard.GetKeyStates(Key.CapsLock) == (KeyStates.Toggled | KeyStates.Down);
			var shiftDown = args.Key == Key.LeftShift || args.Key == Key.RightShift;

			_homeVm.IsUpperKeys = capsToggled ^ shiftDown;
		}

		private void KeyUp(KeyEventArgs args)
		{
			if (args.Key == Key.LeftCtrl || args.Key == Key.RightCtrl)
			{
				return;
			}
			var capsToggled = Keyboard.GetKeyStates(Key.CapsLock) == KeyStates.Toggled;
			var shiftUp = args.Key == Key.LeftShift || args.Key == Key.RightShift;

			_homeVm.IsUpperKeys = (capsToggled && shiftUp) || (capsToggled && !shiftUp);
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

		private void MoveWindow(Window window)
		{
			window.DragMove();
		}
	}
}
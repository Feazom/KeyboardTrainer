using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;

namespace KeyboardTrainer.ViewModel
{
	public class MainViewModel : ViewModelBase
	{
		public RelayCommand HomeViewCommand { get; set; }
		public RelayCommand OptionsViewCommand { get; set; }
		public RelayCommand<Window> CloseWindowCommand { get; set; }
		public RelayCommand<Window> MinimizeWindowCommand { get; set; }
		public RelayCommand<Window> MoveWindowCommand { get; set; }

		private readonly HomeViewModel _homeVM;
		private readonly SettingsViewModel _optionsVM;

		private object _currentView;

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

		public MainViewModel()
		{
			_homeVM = ViewModelLocator.GetViewModel<HomeViewModel>();
			_optionsVM = ViewModelLocator.GetViewModel<SettingsViewModel>();

			CurrentView = _homeVM;

			HomeViewCommand = new RelayCommand(ViewToHome);
			OptionsViewCommand = new RelayCommand(ViewToOptions);
			CloseWindowCommand = new RelayCommand<Window>(Close);
			MinimizeWindowCommand = new RelayCommand<Window>(MinimizeWindow);
			MoveWindowCommand = new RelayCommand<Window>(MoveWindow);
		}

		private void ViewToHome()
		{
			CurrentView = _homeVM;
		}

		private void ViewToOptions()
		{
			CurrentView = _optionsVM;
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
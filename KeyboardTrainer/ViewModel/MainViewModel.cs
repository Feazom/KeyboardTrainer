using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KeyboardTrainer.Model;
using System.Windows;

namespace KeyboardTrainer.ViewModel
{
	public class MainViewModel : ViewModelBase
	{
		public MainViewModel()
		{
			_ = Vocabularies.Instance;

			CurrentView = ViewModelLocator.GetViewModel<HomeViewModel>();

			HomeViewCommand = new RelayCommand(ChangeViewTo<HomeViewModel>);
			SettingsViewCommand = new RelayCommand(ChangeViewTo<SettingsViewModel>);
			StatisticViewCommand = new RelayCommand(ChangeViewTo<StatisticViewModel>);
			CloseWindowCommand = new RelayCommand<Window>(Close);
			MinimizeWindowCommand = new RelayCommand<Window>(MinimizeWindow);
			MoveWindowCommand = new RelayCommand<Window>(MoveWindow);
		}

		#region Properties
		public RelayCommand HomeViewCommand { get; }
		public RelayCommand SettingsViewCommand { get; }
		public RelayCommand StatisticViewCommand { get; }
		public RelayCommand<Window> CloseWindowCommand { get; }
		public RelayCommand<Window> MinimizeWindowCommand { get; }
		public RelayCommand<Window> MoveWindowCommand { get; }

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
using KeyboardTrainer.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KeyboardTrainer.View
{
	/// <summary>
	/// Логика взаимодействия для HomeView.xaml
	/// </summary>
	public partial class HomeView : UserControl
	{
		public HomeView()
		{
			InitializeComponent();
		}

		private void FocusOnMouseDown(object sender, MouseButtonEventArgs e)
		{
			((UIElement)sender).Focus();
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			ViewModelLocator.GetViewModel<HomeViewModel>().LoadedCommand.Execute(null);
		}
	}
}

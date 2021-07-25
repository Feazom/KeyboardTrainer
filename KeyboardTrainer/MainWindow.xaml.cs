using KeyboardTrainer.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace KeyboardTrainer
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			PreviewKeyDown += Preview;
		}

		private void Preview(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Tab || e.Key == Key.Left || e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Right)
			{
				e.Handled = true;
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			ViewModelLocator.GetViewModel<MainViewModel>().ClosingCommand.Execute(null);
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			ViewModelLocator.GetViewModel<MainViewModel>().KeyDownCommand.Execute(e);
		}

		private void Window_KeyUp(object sender, KeyEventArgs e)
		{
			ViewModelLocator.GetViewModel<MainViewModel>().KeyUpCommand.Execute(e);
		}

		private void Window_TextInput(object sender, TextCompositionEventArgs e)
		{
			ViewModelLocator.GetViewModel<MainViewModel>().TextInputCommand.Execute(e);
		}

		private void Control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			DragMove();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}

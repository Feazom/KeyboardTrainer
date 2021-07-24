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
	}
}

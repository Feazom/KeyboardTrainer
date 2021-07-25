using KeyboardTrainer.ViewModel;
using System.Windows.Controls;

namespace KeyboardTrainer.View
{
	/// <summary>
	/// Логика взаимодействия для StatisticView.xaml
	/// </summary>
	public partial class StatisticView : UserControl
	{
		public StatisticView()
		{
			InitializeComponent();
		}

		private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			ViewModelLocator.GetViewModel<StatisticViewModel>().LoadedCommand.Execute(null);
		}
	}
}

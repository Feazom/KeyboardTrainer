using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using KeyboardTrainer.Model;
using OxyPlot;
using OxyPlot.Axes;
using System.Linq;

namespace KeyboardTrainer.ViewModel
{
	public class StatisticViewModel : ViewModelBase
	{
		public StatisticViewModel()
		{
			Model = new PlotModel
			{
				Title = "Количество клавиш в минуту",
			};

			LoadedCommand = new RelayCommand(Load);
		}

		#region Properties
		public RelayCommand LoadedCommand { get; }

		public PlotModel Model
		{
			get => _model;
			private set
			{
				_model = value;
				RaisePropertyChanged(nameof(Model));
			}
		}
		#endregion

		private PlotModel _model;

		private void Load()
		{
			var statistic = new Statistic();

			Model.Axes.Add(new DateTimeAxis
			{
				Position = AxisPosition.Bottom,
				Title = "Время установки результата",
				Minimum = Axis.ToDouble(statistic.Data
					.Select(n => n.Time)
					.Min()
					.ToLocalTime())
			});

			Model.Axes.Add(new LinearAxis
			{
				Position = AxisPosition.Left,
				Title = "Клавиш в минуту"
			});
		}
	}
}

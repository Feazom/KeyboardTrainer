using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using KeyboardTrainer.Core;
using KeyboardTrainer.Model;
using OxyPlot;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KeyboardTrainer.ViewModel
{
	public class StatisticViewModel : ViewModelBase
	{
		public StatisticViewModel()
		{
			_intervals = new Dictionary<string, Interval>
			{
				{ "Час" , new Interval(TimeSpan.FromHours(1), "HH:mm:ss\n")},
				{ "Сутки" , new Interval(TimeSpan.FromHours(24), "HH:mm\n")},
				{ "Неделя" , new Interval(TimeSpan.FromDays(7), "HH:mm\ndd/MM")},
				{ "Месяц" , new Interval(TimeSpan.FromDays(30), "HH:mm\ndd/MM")}
			};
			SelectedInterval = _intervals.Keys.FirstOrDefault();
			SelectedVocabulary = Vocabularies.Instance.Current.Name;

			LoadedCommand = new RelayCommand(Load);
		}

		#region Properties
		public RelayCommand LoadedCommand { get; }

		public string DateTimeFormat
		{
			get => _dateTimeFormat;
			private set
			{
				_dateTimeFormat = value;
				RaisePropertyChanged(nameof(DateTimeFormat));
			}
		}

		public int MaxCharPerMinute
		{
			get => _maxCharPerMinute + 200;
			private set
			{
				_maxCharPerMinute = value;
				RaisePropertyChanged(nameof(MaxCharPerMinute));
			}
		}

		public IEnumerable<DataPoint> PointsCharPerMinute
		{
			get => _pointsCharPerMinute;
			private set
			{
				_pointsCharPerMinute = value;
				RaisePropertyChanged(nameof(PointsCharPerMinute));
			}
		}

		public IEnumerable<DataPoint> PointsErrors
		{
			get => _pointsErrors;
			private set
			{
				_pointsErrors = value;
				RaisePropertyChanged(nameof(PointsErrors));
			}
		}

		public string SelectedVocabulary
		{
			get => _selectedVocabulary;
			set
			{
				_selectedVocabulary = value;
				Load();
			}
		}

		public string SelectedInterval
		{
			get => _selectedInterval;
			set
			{
				_selectedInterval = value;
				Load();
			}
		}

		public double MaxTimeInDouble => DateTimeAxis.ToDouble(_maxTime + TimeSpan.FromSeconds(1));
		public double MinTimeInDouble => DateTimeAxis.ToDouble(_minTime - TimeSpan.FromSeconds(1));
		public string[] VocabularyList => Vocabularies.Instance.Collection.Select(n => n.Name).ToArray();
		public string[] IntervalList => _intervals.Keys.ToArray();
		#endregion

		private DateTime _maxTime;
		private DateTime _minTime;
		private readonly Dictionary<string, Interval> _intervals;
		private string _dateTimeFormat;
		private string _selectedInterval;
		private string _selectedVocabulary;
		private int _maxCharPerMinute;
		private IEnumerable<DataPoint> _pointsCharPerMinute;
		private IEnumerable<DataPoint> _pointsErrors;

		private void Load()
		{
			var statistic = new Statistic().Data
				.Where(n => n.Vocabulary == SelectedVocabulary)
				.OrderBy(n => n.Time);

			DateTimeFormat = _intervals[SelectedInterval].TimeFormat;

			SetMaxTime(DateTime.Now);
			SetMinTime(DateTime.Now - _intervals[SelectedInterval].TimeSpan);

			if (!statistic.Any())
			{
				PointsCharPerMinute = null;
				PointsErrors = null;
				return;
			}


			MaxCharPerMinute = statistic
				.Select(n => n.CharPerMinute)
				.Max();
			PointsCharPerMinute = statistic
				.Where(n => n.Time >= _minTime && n.Time <= _maxTime)
				.Select(n => new DataPoint(DateTimeAxis.ToDouble(n.Time.ToLocalTime().DateTime), n.CharPerMinute));
			PointsErrors = statistic
				.Where(n => n.Time >= _minTime && n.Time <= _maxTime)
				.Select(n => new DataPoint(DateTimeAxis.ToDouble(n.Time.ToLocalTime().DateTime), n.ErrorsPercent));
		}

		private void SetMinTime(DateTime value)
		{
			_minTime = value;
			RaisePropertyChanged(nameof(MinTimeInDouble));
		}

		private void SetMaxTime(DateTime value)
		{
			_maxTime = value;
			RaisePropertyChanged(nameof(MaxTimeInDouble));
		}
	}
}
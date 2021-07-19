using OxyPlot.Axes;
using System;

namespace KeyboardTrainer.Model
{
	public struct Interval
	{
		public Interval(TimeSpan timeSpan, DateTimeIntervalType step)
		{
			TimeSpan = timeSpan;
			Step = step;
		}

		public TimeSpan TimeSpan { get; }
		public DateTimeIntervalType Step { get; }
	}
}

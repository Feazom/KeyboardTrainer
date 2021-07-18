using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

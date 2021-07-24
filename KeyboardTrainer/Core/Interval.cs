using System;

namespace KeyboardTrainer.Core
{
	public struct Interval
	{
		public Interval(TimeSpan timeSpan, string timeFormat)
		{
			TimeSpan = timeSpan;
			TimeFormat = timeFormat;
		}

		public TimeSpan TimeSpan { get; }
		public string TimeFormat { get; }
	}
}

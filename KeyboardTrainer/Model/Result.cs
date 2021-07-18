using System;
using System.Collections.Generic;

namespace KeyboardTrainer.Model
{
	public class Result
	{
		public string Vocabulary { get; set; }
		public int CharPerMinute { get; set; }
		public double ErrorsPercent { get; set; }
		public long DurationTicks { get; set; }
		public DateTimeOffset Time { get; set; }

		public Result(string vocabulary, int charPerMinute, double errorsPercent, long durationTicks, DateTimeOffset time)
		{
			Vocabulary = vocabulary;
			CharPerMinute = charPerMinute;
			ErrorsPercent = errorsPercent;
			DurationTicks = durationTicks;
			Time = time;
		}
	}
}

using System;

namespace KeyboardTrainer.Model
{
	public class Result
	{
		public int CharPerMinute { get; set; }
		public double ErrorsPercent { get; set; }
		public TimeSpan Duration { get; set; }
		public DateTimeOffset Time { get; set; }

		public Result(int charPerMinute, double errorsPercent, TimeSpan duration, DateTimeOffset time)
		{
			CharPerMinute = charPerMinute;
			ErrorsPercent = errorsPercent;
			Duration = duration;
			Time = time;
		}
	}
}

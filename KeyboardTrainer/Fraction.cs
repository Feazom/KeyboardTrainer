using System;

namespace KeyboardTrainer
{
	public class Fraction
	{
		public double Numerator { get; set; }
		public double Denominator { get; set; }

		public Fraction(int numerator = 0, int denominator = 1)
		{
			Numerator = numerator;
			Denominator = denominator;
		}

		public double GetPercent()
		{
			return Math.Round((Numerator / Denominator) * 100, 1);
		}
	}
}

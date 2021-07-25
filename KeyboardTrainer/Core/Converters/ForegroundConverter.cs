using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace KeyboardTrainer.Core.Converters
{
	public class ForegroundConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var border = (Border)values[0];
			var borderColor = ((SolidColorBrush)border.Background).Color;
			var brightness = Brightness(borderColor);
			var foreground = brightness > 127 ? Brushes.Black : Brushes.White;

			return foreground;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		private int Brightness(Color c)
		{
			return (int)Math.Sqrt(
			   (c.R * c.R * .241) +
			   (c.G * c.G * .691) +
			   (c.B * c.B * .068));
		}

		[DllImport("user32.dll")]
		private static extern short VkKeyScan(char ch);
	}
}

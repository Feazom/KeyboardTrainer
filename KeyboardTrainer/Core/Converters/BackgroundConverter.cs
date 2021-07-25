using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace KeyboardTrainer.Core.Converters
{
	public class BackgroundConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var tag = (string)values[0];
			var curentBackground = (SolidColorBrush)values[2];

			var requiredKeyChar = char.ToLower((char)values[1]);
			var requiredKeyCode = VkKeyScan(requiredKeyChar);
			var requiredKey = KeyInterop.KeyFromVirtualKey(requiredKeyCode);

			var isShift = tag == "SHIFT" && requiredKey != Key.Space;
			var isCaps = tag == "CAPS" && requiredKey != Key.Space;
			var isDifferentCase = char.IsUpper((char)values[1]) != (bool)values[3] && requiredKey != Key.Space;
			var isRightKey = tag.Equals(requiredKey.ToString(), StringComparison.CurrentCultureIgnoreCase);

			if (isRightKey || (isDifferentCase && (isShift || isCaps)))
			{
				if (isCaps)
				{
					return Keyboard.GetKeyStates(Key.CapsLock) == KeyStates.Toggled ||
						Keyboard.GetKeyStates(Key.CapsLock) == (KeyStates.Toggled | KeyStates.Down)
						? new SolidColorBrush(ChangeColorBrightness(curentBackground.Color, -0.7f))
						: curentBackground;
				}

				return new SolidColorBrush(ChangeColorBrightness(curentBackground.Color, -0.7f));
			}

			return curentBackground;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		private Color ChangeColorBrightness(Color color, float correctionFactor)
		{
			float red = (float)color.R;
			float green = (float)color.G;
			float blue = (float)color.B;

			if (correctionFactor < 0)
			{
				correctionFactor = 1 + correctionFactor;
				red *= correctionFactor;
				green *= correctionFactor;
				blue *= correctionFactor;
			}
			else
			{
				red = (255 - red) * correctionFactor + red;
				green = (255 - green) * correctionFactor + green;
				blue = (255 - blue) * correctionFactor + blue;
			}

			return Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);
		}

		[DllImport("user32.dll")]
		private static extern short VkKeyScan(char ch);
	}
}

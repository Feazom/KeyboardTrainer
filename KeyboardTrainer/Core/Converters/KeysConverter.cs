using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace KeyboardTrainer.Core.Converters
{
	public class KeysConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var tag = values[0] as string;

			if (tag == "SPACE" || !Enum.TryParse<Key>(tag, true, out var key))
			{
				return tag;
			}

			return KeyboardConvert.GetCharFromKey(key);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

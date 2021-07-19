using KeyboardTrainer.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace KeyboardTrainer.Model
{
	public class KeyConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var tag = (string)values[0];

			if (!Enum.TryParse<Key>(tag, out var key))
			{
				key = KeyInterop.KeyFromVirtualKey(VkKeyScan(tag[0]));
			}

			return char.ToUpper(KeyboardConvert.GetCharFromKey(key));
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		[DllImport("user32.dll")] 
		private static extern short VkKeyScan(char ch);
	}
}

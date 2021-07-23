using System.Windows;

namespace KeyboardTrainer.View
{
	public class AutoForegroundExtension : DependencyObject
	{
		public static readonly DependencyProperty AutoForegroundProperty = DependencyProperty.RegisterAttached(
			"AutoForeground",
			typeof(bool),
			typeof(AutoForegroundExtension),
			new PropertyMetadata(false)
		);

		public static void SetAutoForeground(DependencyObject element, bool value)
		{
			element.SetValue(AutoForegroundProperty, value);
		}
		public static bool GetAutoForeground(DependencyObject element)
		{
			return (bool)element.GetValue(AutoForegroundProperty);
		}
		//private static void OnAutoForegroundPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		//{
		//	if (d is null || !(e.NewValue is bool) || !(bool)e.NewValue)
		//	{
		//		return;
		//	}

		//	var control = (Control)d;
		//	var border = (Border)control.Parent;
		//	var parentColor = ((SolidColorBrush)border.Background).Color;
		//	var brightness = Brightness(parentColor);
		//	var foreground = brightness > 127 ? Brushes.Black : Brushes.White;

		//	control.Foreground = foreground;
		//}
		//private static int Brightness(Color c)
		//{
		//	return (int)Math.Sqrt(
		//	   (c.R * c.R * .241) +
		//	   (c.G * c.G * .691) +
		//	   (c.B * c.B * .068));
		//}
	}
}

/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:KeyboardTrainer"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace KeyboardTrainer.ViewModel
{
	/// <summary>
	/// This class contains static references to all the view models in the
	/// application and provides an entry point for the bindings.
	/// </summary>
	public class ViewModelLocator
	{
		/// <summary>
		/// Initializes a new instance of the ViewModelLocator class.
		/// </summary>
		public ViewModelLocator()
		{
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

			SimpleIoc.Default.Register<MainViewModel>();
			SimpleIoc.Default.Register<HomeViewModel>();
			SimpleIoc.Default.Register<OptionsViewModel>();
		}

		public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
		public HomeViewModel Home => ServiceLocator.Current.GetInstance<HomeViewModel>();
		public OptionsViewModel Options => ServiceLocator.Current.GetInstance<OptionsViewModel>();

		public static T GetViewModel<T>() where T : ViewModelBase
		{
			return ServiceLocator.Current.GetInstance<T>();
		}

		public static void Cleanup()
		{
			SimpleIoc.Default.Unregister<MainViewModel>();
			SimpleIoc.Default.Unregister<HomeViewModel>();
			SimpleIoc.Default.Unregister<OptionsViewModel>();
		}
	}
}
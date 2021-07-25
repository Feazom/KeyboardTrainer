using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using KeyboardTrainer.Core.Audio;
using KeyboardTrainer.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace KeyboardTrainer.ViewModel
{
	public class SettingsViewModel : ViewModelBase
	{
		public SettingsViewModel()
		{
			VocabularyList = Vocabularies.Instance.Collection.Select(n => n.Name).ToList();
			SelectedVocabulary = Vocabularies.Instance.Current.Name;
			_volume = 100;

			SaveSettingCommand = new RelayCommand(SaveSetting);
			//VolumeChangedCommand = new RelayCommand<RoutedPropertyChangedEventArgs<double>>(VolumeChanged);
		}

		#region Properties
		public RelayCommand SaveSettingCommand { get; }
		//public RelayCommand<RoutedPropertyChangedEventArgs<double>> VolumeChangedCommand { get; }

		public List<string> VocabularyList { get; }

		public string SelectedVocabulary
		{
			get => _selectedVocabulary;
			set
			{
				_selectedVocabulary = value;
				Vocabularies.Instance.SetCurrentTo(_selectedVocabulary);
				ViewModelLocator.GetViewModel<HomeViewModel>().Reset();
			}
		}

		public double Volume
		{
			get => _volume;
			set
			{
				_volume = value;
				AudioPlayer.Instance.Volume((float)_volume);
				RaisePropertyChanged(nameof(Volume));
			}
		}
		#endregion

		private string _selectedVocabulary;
		private double _volume;

		//private void VolumeChanged(RoutedPropertyChangedEventArgs<double> args)
		//{
		//	AudioPlayer.Instance.Volume((float)args.NewValue);
		//}

		private void SaveSetting()
		{
			var settings = Settings.Load();
			settings.SelectedVocabulary = SelectedVocabulary;
			settings.Save();
		}
	}
}

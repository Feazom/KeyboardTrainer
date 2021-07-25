using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using KeyboardTrainer.Core.Audio;
using KeyboardTrainer.Model;
using System.Collections.Generic;
using System.Linq;

namespace KeyboardTrainer.ViewModel
{
	public class SettingsViewModel : ViewModelBase
	{
		public SettingsViewModel()
		{
			var volume = Properties.Settings.Default.Volume;
			Volume = volume == -1 ? 1f : volume;

			VocabularyList = Vocabularies.Instance.Collection.Select(n => n.Name).ToList();
			SelectedVocabulary = Vocabularies.Instance.Current.Name;

			SaveSettingCommand = new RelayCommand(SaveSetting);
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

		public float Volume
		{
			get => _volume;
			set
			{
				_volume = value;
				AudioPlayer.Instance.Volume(_volume);
				RaisePropertyChanged(nameof(Volume));
			}
		}
		#endregion

		private string _selectedVocabulary;
		private float _volume;

		public void SaveSetting()
		{
			Properties.Settings.Default.Vocabulary = SelectedVocabulary;
			Properties.Settings.Default.Volume = Volume;

			Properties.Settings.Default.Save();
		}
	}
}

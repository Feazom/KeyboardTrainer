using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using KeyboardTrainer.Model;
using System.Collections.Generic;
using System.Linq;

namespace KeyboardTrainer.ViewModel
{
	public class SettingsViewModel : ViewModelBase
	{
		public RelayCommand SaveSettingCommand { get; set; }

		public List<string> VocabularyList { get; }

		public string SelectedVocabulary
		{
			get => _selectedVocabulary;
			set
			{
				_selectedVocabulary = value;
				Vocabularies.Instance.SetCurrentTo(_selectedVocabulary);
			}
		}

		private string _selectedVocabulary;

		public SettingsViewModel()
		{
			VocabularyList = Vocabularies.Instance.Collection.Select(n => n.Name).ToList();
			SelectedVocabulary = Vocabularies.Instance.Current.Name;

			SaveSettingCommand = new RelayCommand(SaveSetting);
		}

		private void SaveSetting()
		{
			var settings = Settings.Load();
			settings.SelectedVocabulary = SelectedVocabulary;
			settings.Save();
		}
	}
}

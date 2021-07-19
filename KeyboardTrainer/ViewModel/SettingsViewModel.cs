using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using KeyboardTrainer.Model;
using System.Collections.Generic;
using System.Linq;

namespace KeyboardTrainer.ViewModel
{
	public class SettingsViewModel : ViewModelBase
	{
		public SettingsViewModel()
		{
			VocabularyList = Vocabularies.Instance.Collection.Select(n => n.Name).ToList();
			SelectedVocabulary = Vocabularies.Instance.Current.Name;

			SaveSettingCommand = new RelayCommand(SaveSetting);
		}

		#region Properties
		public RelayCommand SaveSettingCommand { get; }

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
		#endregion

		private string _selectedVocabulary;

		private void SaveSetting()
		{
			var settings = Settings.Load();
			settings.SelectedVocabulary = SelectedVocabulary;
			settings.Save();
		}
	}
}

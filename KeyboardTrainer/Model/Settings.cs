using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KeyboardTrainer.Model
{
	public class Settings
	{
		public string SelectedVocabulary { get; set; }
		[JsonIgnore]
		public bool IsEmpty { get; private set; } = false;

		private const string SettingsFileName = "settings.json";

		public void Save()
		{
			IsEmpty = false;
			var jsonString = JsonSerializer.Serialize(this);
			File.WriteAllText(SettingsFileName, jsonString);
		}

		public static Settings Load()
		{
			if (!File.Exists(SettingsFileName))
			{
				File.Create(SettingsFileName);
			}
			try
			{
				var jsonString = File.ReadAllText(SettingsFileName);
				return JsonSerializer.Deserialize<Settings>(jsonString);
			}
			catch (JsonException)
			{
				return new Settings { IsEmpty = true };
			}
		}
	}
}

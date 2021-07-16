using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace KeyboardTrainer.Model
{
	public class Statistic
	{
		public List<Result> Data { get; set; }

		private const string _statsFileName = "stats.json";

		public Statistic()
		{
			Data = new List<Result>();

			DeserializeData();
		}

		public void Add(Result result)
		{
			Data.Add(result);
		}

		public void Save()
		{
			SerializeData();
		}

		private void SerializeData()
		{
			var jsonString = JsonSerializer.Serialize(Data);
			File.WriteAllText(_statsFileName, jsonString);
		}

		private void DeserializeData()
		{
			try
			{
				var jsonString = File.ReadAllText(_statsFileName);
				Data = JsonSerializer.Deserialize<List<Result>>(jsonString);
			}
			catch (JsonException)
			{
				return;
			}
		}
	}
}

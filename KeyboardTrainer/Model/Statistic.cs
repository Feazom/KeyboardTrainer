using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace KeyboardTrainer.Model
{
	public class Statistic
	{
		public List<Result> Data { get; private set; }

		private const string StatsFileName = "stats.json";

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
			var jsonString = JsonSerializer.Serialize(
				Data,
				new JsonSerializerOptions { WriteIndented = true });

			File.WriteAllText(StatsFileName, jsonString);
		}

		private void DeserializeData()
		{
			try
			{
				if (!File.Exists(StatsFileName))
				{
					File.Create(StatsFileName);
				}
				var jsonString = File.ReadAllText(StatsFileName);
				Data = JsonSerializer.Deserialize<List<Result>>(jsonString);
			}
			catch (JsonException)
			{
				return;
			}
		}
	}
}

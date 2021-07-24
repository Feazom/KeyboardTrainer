using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace KeyboardTrainer.Model
{
	public class Vocabularies
	{
		public static Vocabularies Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (_syncRoot)
					{
						if (_instance == null)
						{
							_instance = new Vocabularies();
						}
					}
				}
				return _instance;
			}
		}

		public List<Vocabulary> Collection { get; private set; }
		public Vocabulary Current { get; private set; }

		private static Vocabularies _instance;
		private static readonly object _syncRoot = new Object();
		private static readonly string VocabularyRoot =
			Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Vocabularies");

		private Vocabularies()
		{
			Collection = new List<Vocabulary>();
			FillVocabularyList();

			var settings = Settings.Load();

			if (settings.SelectedVocabulary == null)
			{
				Current = Collection.FirstOrDefault();
			}
			else
			{
				SetCurrentTo(settings.SelectedVocabulary);
			}
		}

		public void SetCurrentTo(string name)
		{
			var current = Collection.Where(n => n.Name == name);

			if (current.Count() != 1)
			{
				Current = Collection.FirstOrDefault();
			}

			Current = current.First();
		}

		public string GetContent(int count, bool allowCapital = false)
		{
			if (Current == default(Vocabulary))
			{
				return null;
			}

			var random = new Random(DateTime.Now.Millisecond);
			var words = Current.Content.Split(' ');
			var resultSequence = new List<string>();

			for (int i = 0; i < count; i++)
			{
				var word = words[random.Next(0, words.Length)];

				if (allowCapital && random.Next(0, 3) == 0)
				{
					word = WordWithCapital(word);
				}
				resultSequence.Add(word);
			}
			return string.Join(" ", resultSequence);
		}

		private void FillVocabularyList()
		{
			var manifestPath = Path.Combine(VocabularyRoot, "manifest.json");
			var jsonString = File.ReadAllText(manifestPath);
			var manifest = JsonSerializer.Deserialize<Manifest>(jsonString);

			foreach (var vocabulary in manifest.Names)
			{
				ReadJSON(vocabulary);
			}
		}

		private void ReadJSON(Names vocabulary)
		{
			var path = Path.Combine(VocabularyRoot, vocabulary.FileName);

			var text = File.ReadAllText(path);
			Collection.Add(new Vocabulary(vocabulary.Name, text));
		}

		private string WordWithCapital(string word)
		{
			return word.Length == 1 ?
				char.ToUpper(word[0]).ToString() :
				char.ToUpper(word[0]) + word.Substring(1);
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace KeyboardTrainer.Model
{
	public class Vocabulary
	{
		public Vocabulary(string name, string content)
		{
			Name = name;
			Content = content;
		}

		public string Name { get; private set; }
		public string Content { get; private set; }


	}
}

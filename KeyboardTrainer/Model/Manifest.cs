using System.Collections.Generic;

namespace KeyboardTrainer.Model
{
	public class Manifest
	{
		public List<Names> Names { get; set; }
	}

	public class Names
	{
		public string Name { get; set; }
		public string FileName { get; set; }
	}
}

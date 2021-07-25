namespace KeyboardTrainer.Core.Audio
{
	public class AudioPlayer
	{
		public static AudioPlayer Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (_syncRoot)
					{
						if (_instance == null)
						{
							_instance = new AudioPlayer();
						}
					}
				}
				return _instance;
			}
		}

		private static AudioPlayer _instance;
		private static readonly object _syncRoot = new object();

		private readonly CachedSound _failSound;

		public void Fail()
		{
			AudioPlaybackEngine.Instance.PlaySound(_failSound);
		}

		public void Volume(float volume)
		{
			AudioPlaybackEngine.Instance.ChangeVolume(volume);
		}

		private AudioPlayer()
		{
			_failSound = new CachedSound("fail.wav");
		}
	}
}
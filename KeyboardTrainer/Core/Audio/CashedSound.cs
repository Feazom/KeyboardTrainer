using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KeyboardTrainer.Core.Audio
{
	public class CachedSound
	{
		public float[] AudioData { get; private set; }
		public WaveFormat WaveFormat { get; private set; }

		private static readonly string AudioRoot =
			Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Audio");

		public CachedSound(string audioFileName)
		{
			using (var audioFileReader = new AudioFileReader(Path.Combine(AudioRoot, audioFileName)))
			{
				var outFormat = new WaveFormat(44100, 2);
				using (var resampler = new MediaFoundationResampler(audioFileReader, outFormat))
				{
					resampler.ResamplerQuality = 60;

					WaveFormat = outFormat;
					var wholeFile = new List<float>((int)(audioFileReader.Length / 4));
					var readBuffer = new float[outFormat.SampleRate * outFormat.Channels];
					int samplesRead;
					while ((samplesRead = audioFileReader.Read(readBuffer, 0, readBuffer.Length)) > 0)
					{
						wholeFile.AddRange(readBuffer.Take(samplesRead));
					}
					AudioData = wholeFile.ToArray();
				}
			}
		}
	}
}

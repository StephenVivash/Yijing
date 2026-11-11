namespace Yijing.Services;

internal static class ToneWaveform
{
	public const int SampleRate = 44100;

	public static short[] CreatePcm(double frequencyHz, int durationMs, double volume)
	{
		int sampleCount = Math.Max(1, (int)(SampleRate * (durationMs / 1000.0)));
		var pcm = new short[sampleCount];
		int fadeSamples = Math.Min(sampleCount / 20, 200);
		double twoPiF = 2.0 * Math.PI * frequencyHz;

		for (int i = 0; i < sampleCount; i++)
		{
			double time = i / (double)SampleRate;
			double sample = Math.Sin(twoPiF * time);
			double envelope = 1.0;
			if (fadeSamples > 0)
			{
				if (i < fadeSamples) envelope = i / (double)fadeSamples;
				else if (i > sampleCount - fadeSamples) envelope = (sampleCount - i) / (double)fadeSamples;
			}
			pcm[i] = (short)Math.Clamp(sample * envelope * volume * short.MaxValue, short.MinValue, short.MaxValue);
		}
		return pcm;
	}

	public static byte[] CreateWave(double frequencyHz, int durationMs, double volume)
	{
		var pcm = CreatePcm(frequencyHz, durationMs, volume);
		int dataSize = pcm.Length * sizeof(short);
		using var stream = new MemoryStream(44 + dataSize);
		using var writer = new BinaryWriter(stream);
		writer.Write("RIFF"u8);
		writer.Write(36 + dataSize);
		writer.Write("WAVEfmt "u8);
		writer.Write(16); // PCM format chunk size
		writer.Write((ushort)1); // PCM
		writer.Write((ushort)1); // Mono
		writer.Write(SampleRate);
		writer.Write(SampleRate * sizeof(short));
		writer.Write((ushort)sizeof(short));
		writer.Write((ushort)16);
		writer.Write("data"u8);
		writer.Write(dataSize);
		foreach (short sample in pcm)
			writer.Write(sample);
		return stream.ToArray();
	}
}

namespace Yijing.Services;

// Feedback for meditation/EEG-only sessions, where no MindCast loop owns the
// cues. Zero retains SoundTrigger's convention for a single crossing tone.
internal sealed class TriggerToneFeedback
{
	private static readonly TimeSpan ComparisonInterval = TimeSpan.FromSeconds(10);
	private DateTime _lastCue;
	private int _channel = -1;
	private bool _waitingForHigh = true;

	public void Reset(DateTime now)
	{
		_lastCue = now;
		_channel = -1;
		_waitingForHigh = true;
	}

	public float? Update(DateTime now, int channel, float band, float low, float high)
	{
		if (!float.IsFinite(band) || !float.IsFinite(low) || !float.IsFinite(high) || low >= high)
			return null;

		if (_channel != channel)
		{
			_channel = channel;
			_waitingForHigh = true;
			_lastCue = now;
		}

		float target = _waitingForHigh ? high : low;
		bool crossed = _waitingForHigh ? band >= high : band <= low;
		if (crossed)
		{
			_waitingForHigh = !_waitingForHigh;
			_lastCue = now;
			return 0.0f;
		}

		if (now - _lastCue < ComparisonInterval)
			return null;
		_lastCue = now;
		return target;
	}
}

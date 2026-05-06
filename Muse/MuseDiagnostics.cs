namespace Muse.Core;

public sealed record MuseEegPacketDiagnostic(
    string SensorName,
    long Count,
    ushort Sequence,
    int? SequenceDelta,
    bool LargeSequenceJump,
    long TotalLargeSequenceJumps,
    double? IntervalMilliseconds,
    double MinMicrovolts,
    double MaxMicrovolts,
    double MeanAbsMicrovolts,
    double RmsMicrovolts);

internal sealed class MuseEegPacketStats
{
    private bool _hasLastSequence;
    private ushort _lastSequence;
    private DateTimeOffset? _lastTimestamp;
    private long _totalLargeSequenceJumps;

    // The Muse sequence appears to be shared across notification streams, so
    // per-electrode deltas above one are not automatically packet drops.
    private const int LargeSequenceJumpThreshold = 8;

    public MuseEegPacketDiagnostic Track(string sensorName, long count, MuseEegPacket packet)
    {
        var now = DateTimeOffset.Now;
        int? sequenceDelta = null;
        var largeSequenceJump = false;
        if (_hasLastSequence)
        {
            sequenceDelta = (packet.Sequence - _lastSequence + 65536) % 65536;
            largeSequenceJump = sequenceDelta.Value > LargeSequenceJumpThreshold;
            if (largeSequenceJump)
            {
                _totalLargeSequenceJumps++;
            }
        }

        var intervalMilliseconds = _lastTimestamp.HasValue ? (now - _lastTimestamp.Value).TotalMilliseconds : (double?)null;
        _lastSequence = packet.Sequence;
        _lastTimestamp = now;
        _hasLastSequence = true;

        var min = double.PositiveInfinity;
        var max = double.NegativeInfinity;
        var absSum = 0.0;
        var squareSum = 0.0;
        foreach (var sample in packet.Samples)
        {
            min = Math.Min(min, sample);
            max = Math.Max(max, sample);
            absSum += Math.Abs(sample);
            squareSum += sample * sample;
        }

        var sampleCount = Math.Max(1, packet.Samples.Length);
        return new MuseEegPacketDiagnostic(
            sensorName,
            count,
            packet.Sequence,
            sequenceDelta,
            largeSequenceJump,
            _totalLargeSequenceJumps,
            intervalMilliseconds,
            min,
            max,
            absSum / sampleCount,
            Math.Sqrt(squareSum / sampleCount));
    }
}

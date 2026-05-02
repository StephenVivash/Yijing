namespace Muse.Core;

public sealed class EegBandPowerTracker
{
    private readonly double[] _buffer;
    private readonly int _sampleRate;
    private readonly object _gate = new();
    private int _nextIndex;
    private int _count;
    private int _version;
    private int _lastCalculatedVersion;
    private MuseBandPowers _lastBands;

    public EegBandPowerTracker(string name, int windowSamples, int sampleRate)
    {
        Name = name;
        _buffer = new double[windowSamples];
        _sampleRate = sampleRate;
    }

    public string Name { get; }

    public void AddSamples(IReadOnlyList<double> samples)
    {
        lock (_gate)
        {
            foreach (var sample in samples)
            {
                _buffer[_nextIndex] = sample;
                _nextIndex = (_nextIndex + 1) % _buffer.Length;
                _count = Math.Min(_count + 1, _buffer.Length);
            }

            _version++;
        }
    }

    public bool TryCalculate(out MuseBandPowers bands)
    {
        double[] snapshot;
        lock (_gate)
        {
            bands = _lastBands;
            if (_count < _buffer.Length || _version == _lastCalculatedVersion)
            {
                return false;
            }

            snapshot = new double[_buffer.Length];
            for (var i = 0; i < snapshot.Length; i++)
            {
                snapshot[i] = _buffer[(_nextIndex + i) % _buffer.Length];
            }

            _lastCalculatedVersion = _version;
        }

        bands = new MuseBandPowers(
            CalculateBandDisplayDb(snapshot, _sampleRate, 1, 4),
            CalculateBandDisplayDb(snapshot, _sampleRate, 4, 8),
            CalculateBandDisplayDb(snapshot, _sampleRate, 8, 13),
            CalculateBandDisplayDb(snapshot, _sampleRate, 13, 30),
            CalculateBandDisplayDb(snapshot, _sampleRate, 30, 45));

        lock (_gate)
        {
            _lastBands = bands;
        }

        return true;
    }

    private static double CalculateBandDisplayDb(double[] samples, int sampleRate, double lowHz, double highHz)
    {
        var n = samples.Length;
        var mean = samples.Average();
        var firstBin = Math.Max(1, (int)Math.Ceiling(lowHz * n / sampleRate));
        var lastBin = Math.Min(n / 2, (int)Math.Floor(highHz * n / sampleRate));
        var binCount = Math.Max(1, lastBin - firstBin + 1);
        var powerSum = 0.0;

        for (var bin = firstBin; bin <= lastBin; bin++)
        {
            var real = 0.0;
            var imaginary = 0.0;
            for (var i = 0; i < n; i++)
            {
                var window = 0.5 - 0.5 * Math.Cos(2.0 * Math.PI * i / (n - 1));
                var sample = (samples[i] - mean) * window;
                var angle = 2.0 * Math.PI * bin * i / n;
                real += sample * Math.Cos(angle);
                imaginary -= sample * Math.Sin(angle);
            }

            powerSum += (real * real + imaginary * imaginary) / n;
        }

        var averagePower = powerSum / binCount;
        return 10.0 * Math.Log10(Math.Max(averagePower, 1e-12));
    }
}

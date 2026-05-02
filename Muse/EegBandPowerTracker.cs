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
            CalculateAbsoluteBandPower(snapshot, _sampleRate, 1, 4),
            CalculateAbsoluteBandPower(snapshot, _sampleRate, 4, 8),
            CalculateAbsoluteBandPower(snapshot, _sampleRate, 7.5, 13),
            CalculateAbsoluteBandPower(snapshot, _sampleRate, 13, 30),
            CalculateAbsoluteBandPower(snapshot, _sampleRate, 30, 44));

        lock (_gate)
        {
            _lastBands = bands;
        }

        return true;
    }

    private static double CalculateAbsoluteBandPower(double[] samples, int sampleRate, double lowHz, double highHz)
    {
        var n = samples.Length;
        var psd = CalculatePowerSpectralDensity(samples, sampleRate);
        var firstBin = Math.Max(1, (int)Math.Ceiling(lowHz * n / sampleRate));
        var lastBin = Math.Min((n / 2) - 1, (int)Math.Floor(highHz * n / sampleRate));
        var powerSum = 0.0;

        for (var bin = firstBin; bin <= lastBin; bin++)
        {
            powerSum += psd[bin];
        }

        return Math.Log10(Math.Max(powerSum, 1e-12));
    }

    private static double[] CalculatePowerSpectralDensity(double[] samples, int sampleRate)
    {
        var n = samples.Length;
        var mean = samples.Average();
        var psd = new double[(n / 2) + 1];
        var windowPower = 0.0;
        var windowedSamples = new double[n];

        for (var i = 0; i < n; i++)
        {
            var window = 0.54 - 0.46 * Math.Cos(2.0 * Math.PI * i / (n - 1));
            windowedSamples[i] = (samples[i] - mean) * window;
            windowPower += window * window;
        }

        for (var bin = 0; bin < psd.Length; bin++)
        {
            var real = 0.0;
            var imaginary = 0.0;
            for (var i = 0; i < n; i++)
            {
                var angle = 2.0 * Math.PI * bin * i / n;
                real += windowedSamples[i] * Math.Cos(angle);
                imaginary -= windowedSamples[i] * Math.Sin(angle);
            }

            psd[bin] = (real * real + imaginary * imaginary) / Math.Max(sampleRate * windowPower, double.Epsilon);
        }

        return psd;
    }

}

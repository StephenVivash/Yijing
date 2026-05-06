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
    private int _samplesSinceLastCalculation;
    private MuseBandPowers _lastBands;

    public EegBandPowerTracker(string name, int windowSamples, int sampleRate, int hopSamples)
    {
        Name = name;
        _buffer = new double[windowSamples];
        _sampleRate = sampleRate;
        HopSamples = hopSamples;
    }

    public string Name { get; }

    public int HopSamples { get; }

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
            _samplesSinceLastCalculation += samples.Count;
        }
    }

    public bool TryCalculate(out MuseBandPowers bands)
    {
        double[] snapshot;
        lock (_gate)
        {
            bands = _lastBands;
            if (_count < _buffer.Length || _version == _lastCalculatedVersion || _samplesSinceLastCalculation < HopSamples)
            {
                return false;
            }

            snapshot = new double[_buffer.Length];
            for (var i = 0; i < snapshot.Length; i++)
            {
                snapshot[i] = _buffer[(_nextIndex + i) % _buffer.Length];
            }

            _lastCalculatedVersion = _version;
            _samplesSinceLastCalculation = 0;
        }

        var psd = CalculatePowerSpectralDensity(snapshot, _sampleRate);
        var binWidth = (double)_sampleRate / snapshot.Length;

        bands = new MuseBandPowers(
            CalculateAbsoluteBandPower(psd, binWidth, snapshot.Length, _sampleRate, 1, 4),
            CalculateAbsoluteBandPower(psd, binWidth, snapshot.Length, _sampleRate, 4, 8),
            CalculateAbsoluteBandPower(psd, binWidth, snapshot.Length, _sampleRate, 7.5, 13),
            CalculateAbsoluteBandPower(psd, binWidth, snapshot.Length, _sampleRate, 13, 30),
            CalculateAbsoluteBandPower(psd, binWidth, snapshot.Length, _sampleRate, 30, 44, true)); // 44 60 80

        lock (_gate)
        {
            _lastBands = bands;
        }

        return true;
    }

    private static double CalculateAbsoluteBandPower(
        double[] psd,
        double binWidth,
        int sampleCount,
        int sampleRate,
        double lowHz,
        double highHz,
        bool limitNarrowSpikes = false)
    {
        var firstBin = Math.Max(1, (int)Math.Ceiling(lowHz * sampleCount / sampleRate));
        var lastBin = Math.Min((sampleCount / 2) - 1, (int)Math.Floor(highHz * sampleCount / sampleRate));
        var powerSum = 0.0;
        var spikeLimit = limitNarrowSpikes ? CalculateSpikeLimit(psd, firstBin, lastBin) : double.PositiveInfinity;

        for (var bin = firstBin; bin <= lastBin; bin++)
        {
            powerSum += Math.Min(psd[bin], spikeLimit) * binWidth;
        }

        return Math.Log10(Math.Max(powerSum, 1e-12));
    }

    private static double CalculateSpikeLimit(double[] psd, int firstBin, int lastBin)
    {
        if (lastBin < firstBin)
        {
            return double.PositiveInfinity;
        }

        var bandBins = new double[lastBin - firstBin + 1];
        for (var bin = firstBin; bin <= lastBin; bin++)
        {
            bandBins[bin - firstBin] = psd[bin];
        }

        Array.Sort(bandBins);
        var median = bandBins[bandBins.Length / 2];
        return Math.Max(median * 3.0, double.Epsilon);
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

            var oneSidedScale = (bin == 0 || bin == n / 2) ? 1.0 : 2.0;
            psd[bin] = oneSidedScale * (real * real + imaginary * imaginary) / Math.Max(sampleRate * windowPower, double.Epsilon);
        }

        return psd;
    }
}

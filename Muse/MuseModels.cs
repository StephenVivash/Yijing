namespace Muse.Core;

public enum MuseSensorKind
{
    Control,
    Eeg,
    Imu,
    Telemetry,
    Raw
}

public sealed record MuseDeviceAdvertisement(ulong BluetoothAddress, string Name, short Rssi, bool ServiceAdvertised)
{
    public string DisplayName => string.IsNullOrWhiteSpace(Name) ? "Muse headset" : Name;
}

public sealed record MuseSensorDefinition(string Name, Guid Uuid, MuseSensorKind Kind);

public sealed record MuseNotification(string Name, Guid Uuid, MuseSensorKind Kind, byte[] Data, long Count);

public readonly record struct MuseEegPacket(ushort Sequence, double[] Samples);

public readonly record struct MuseBandPowers(double DeltaDb, double ThetaDb, double AlphaDb, double BetaDb, double GammaDb)
{
    public double DeltaOsc => ToMindMonitorOscScale(DeltaDb);

    public double ThetaOsc => ToMindMonitorOscScale(ThetaDb);

    public double AlphaOsc => ToMindMonitorOscScale(AlphaDb);

    public double BetaOsc => ToMindMonitorOscScale(BetaDb);

    public double GammaOsc => ToMindMonitorOscScale(GammaDb);

    private static double ToMindMonitorOscScale(double displayDb) => (displayDb / 50.0) - 1.0;
}

public sealed record MuseBandPowerReading(string SensorName, MuseBandPowers Bands);

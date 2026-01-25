using OverLab.Providers.Abstractions;
using OverLab.Providers.Models;
using iRacingSdkWrapper;

namespace OverLab.Providers.iRacing;

public class IracingTelemetryProvider : ITelemetryProvider
{
    public event Action<PedalTelemetry>? PedalTelemetryUpdated;

    private readonly SdkWrapper _wrapper;

    public IracingTelemetryProvider()
    {
        _wrapper = new SdkWrapper
        {
            TelemetryUpdateFrequency = 60
        };

        _wrapper.TelemetryUpdated += OnTelemetryUpdated;
    }

    public void Start()
    {
        _wrapper.Start();
    }

    public void Stop()
    {
        _wrapper.Stop();
    }

    private void OnTelemetryUpdated(
        object sender,
        SdkWrapper.TelemetryUpdatedEventArgs e)
    {
        var telemetry = e.TelemetryInfo;

        float throttle = telemetry.Throttle.Value;
        float brake    = telemetry.Brake.Value;
        float clutch   = telemetry.ClutchRaw.Value;

        // ⚠️ CLUTCH INVERTIDO EN iRACING
        clutch = 1.0f - clutch;

        PedalTelemetryUpdated?.Invoke(new PedalTelemetry
        {
            Throttle = Math.Clamp(throttle, 0f, 1f),
            Brake    = Math.Clamp(brake, 0f, 1f),
            Clutch   = Math.Clamp(clutch, 0f, 1f)
        });
    }
}
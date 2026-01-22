using OverLab.Core.Widgets;
using OverLab.Providers.Abstractions;

namespace OverLab.Core.Telemetry;

public class TelemetryManager
{
    private readonly ITelemetryProvider _provider;

    public TelemetryManager(
        ITelemetryProvider provider,
        PedalTraceWidget pedalWidget)
    {
        _provider = provider;
        _provider.PedalTelemetryUpdated += pedalWidget.Update;
    }

    public void Start() => _provider.Start();
    public void Stop() => _provider.Stop();
}
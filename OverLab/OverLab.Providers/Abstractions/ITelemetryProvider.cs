using OverLab.Providers.Models;

namespace OverLab.Providers.Abstractions;

public interface ITelemetryProvider
{
    event Action<PedalTelemetry> PedalTelemetryUpdated;
    void Start();
    void Stop();
}
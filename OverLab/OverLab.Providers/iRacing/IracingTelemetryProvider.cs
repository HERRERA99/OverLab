using System.Timers;
using OverLab.Providers.Abstractions;
using OverLab.Providers.Models;
using System.Timers;

namespace OverLab.Providers.iRacing;

public class IracingTelemetryProvider : ITelemetryProvider
{
    public event Action<PedalTelemetry>? PedalTelemetryUpdated;

    private readonly System.Timers.Timer _timer;
    
    private float _throttle;
    private float _brake;
    private float _clutch;

    public IracingTelemetryProvider()
    {
        _timer = new System.Timers.Timer(50);
        _timer.Elapsed += OnTimerElapsed;
    }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        _throttle = SmoothRandom(_throttle);
        _brake    = SmoothRandom(_brake);
        _clutch   = SmoothRandom(_clutch);

        PedalTelemetryUpdated?.Invoke(new PedalTelemetry
        {
            Throttle = _throttle,
            Brake = _brake,
            Clutch = _clutch
        });
    }

    private static float SmoothRandom(float current)
    {
        float delta = (Random.Shared.NextSingle() - 0.5f) * 0.1f;
        float next = current + delta;

        return Math.Clamp(next, 0f, 1f);
    }

    public void Start() => _timer.Start();
    public void Stop() => _timer.Stop();
}
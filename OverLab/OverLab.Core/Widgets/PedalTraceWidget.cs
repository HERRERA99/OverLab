using OverLab.Core.Telemetry;
using OverLab.Providers.Models;

namespace OverLab.Core.Widgets;

public class PedalTraceWidget : IPedalTraceWidget
{
    private readonly int _maxSamples;
    private readonly Queue<PedalSample> _samples;

    public event Action<IReadOnlyList<PedalSample>>? SamplesUpdated;

    public PedalTraceWidget(int maxSamples = 50)
    {
        _maxSamples = maxSamples;
        _samples = new Queue<PedalSample>(maxSamples);
    }

    public void Update(PedalTelemetry telemetry)
    {
        if (_samples.Count == _maxSamples)
            _samples.Dequeue();

        _samples.Enqueue(new PedalSample(
            telemetry.Throttle,
            telemetry.Brake,
            telemetry.Clutch));

        SamplesUpdated?.Invoke(_samples.ToArray());
    }
}

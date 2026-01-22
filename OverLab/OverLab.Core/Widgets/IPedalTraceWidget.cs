using OverLab.Core.Telemetry;

namespace OverLab.Core.Widgets;

public interface IPedalTraceWidget : IWidget
{
    event Action<IReadOnlyList<PedalSample>> SamplesUpdated;
}
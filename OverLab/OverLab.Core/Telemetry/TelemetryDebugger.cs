using OverLab.Core.Widgets;

namespace OverLab.Core.Telemetry;

public class TelemetryDebugger
{
    public TelemetryDebugger(IPedalTraceWidget widget)
    {
        widget.SamplesUpdated += samples =>
        {
            var s = samples[^1];
            Console.WriteLine(
                $"[DEBUG] T:{s.Throttle:0.00} B:{s.Brake:0.00} C:{s.Clutch:0.00}");
        };
    }
}
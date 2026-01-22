namespace OverLab.Core.App;

public class AppState
{
    public Dictionary<OverlayId, OverlayConfiguration> Overlays { get; } =
        new()
        {
            { OverlayId.Pedals, new OverlayConfiguration() }
        };
}
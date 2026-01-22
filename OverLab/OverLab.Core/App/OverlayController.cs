namespace OverLab.Core.App;


public class OverlayController
{
    public AppState State { get; } = new();

    public event Action<OverlayId>? OverlayVisibilityChanged;

    public void SetOverlayVisible(OverlayId id, bool visible)
    {
        State.Overlays[id].IsVisible = visible;
        OverlayVisibilityChanged?.Invoke(id);
    }

    public OverlayConfiguration GetOverlay(OverlayId id)
        => State.Overlays[id];
}
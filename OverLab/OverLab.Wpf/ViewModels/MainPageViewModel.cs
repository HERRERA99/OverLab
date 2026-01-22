using System.Diagnostics;
using OverLab.Core.App;

namespace OverLab.Wpf.ViewModels;

public class MainPageViewModel
{
    private readonly OverlayController _controller;

    public bool ShowPedalOverlay
    {
        get => _controller.GetOverlay(OverlayId.Pedals).IsVisible;
        set 
        {
            // Cambiamos el estado
            _controller.SetOverlayVisible(OverlayId.Pedals, value);
            
            // Imprimimos el log en la ventana de "Salida" (Output) de Visual Studio
            Console.WriteLine($"[DEBUG] Pedals Overlay: {value}");
        }
    }

    public MainPageViewModel(OverlayController controller)
    {
        _controller = controller;
    }
}
using OverLab.Wpf.ViewModels;
using OverLab.Wpf.Views.Overlays;
using System.Collections.Generic;
using System.Windows;
using OverLab.Core.App;

namespace OverLab.Wpf.ViewModels;

public class OverlayFactory
{
    private readonly OverlayController _controller;

    public OverlayFactory(OverlayController controller)
    {
        _controller = controller;
    }

    public Window CreateOverlay(string id, IViewModel vm)
    {
        return id switch
        {
            "Pedals" => new OverlayInputsView((OverlayInputsViewModel)vm),
            _ => throw new KeyNotFoundException($"Overlay '{id}' not implemented")
        };
    }
}
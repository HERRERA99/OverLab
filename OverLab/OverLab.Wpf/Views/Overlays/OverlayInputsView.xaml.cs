using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using OverLab.Wpf.Rendering;
using OverLab.Wpf.ViewModels;

namespace OverLab.Wpf.Views.Overlays;

public partial class OverlayInputsView : Window
{
    private readonly OverlayInputsViewModel _vm;

    public OverlayInputsView(OverlayInputsViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        DataContext = vm;

        ApplyVisibility(vm.IsVisible);

        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(vm.IsVisible))
                ApplyVisibility(vm.IsVisible);
        };

        // Cada frame de WPF
        CompositionTarget.Rendering += OnRendering;

        // Redibuja rejilla si cambia el tamaño
        SizeChanged += (_, _) => DrawGrid();

        // Dibuja la rejilla inicial
        Loaded += (_, _) => DrawGrid();
    }

    private void OnRendering(object? sender, EventArgs e)
    {
        if (!IsVisible)
            return;

        Redraw();
    }


    private void ApplyVisibility(bool visible)
    {
        if (visible && !IsVisible) Show();
        else if (!visible && IsVisible) Hide();
    }

    private void Redraw()
    {
        double w = ActualWidth - 2;
        double h = ActualHeight - 2;

        // Acelerador y Freno se dibujan normal (incluyendo el 0)
        ThrottlePath.Data = SplineHelper.CreateSmoothCurve(
            _vm.Samples.Select(s => s.Throttle).ToList(), w, h);

        BrakePath.Data = SplineHelper.CreateSmoothCurve(
            _vm.Samples.Select(s => s.Brake).ToList(), w, h);

        // ESTRATEGIA PARA EL EMBRAGUE:
        // Filtramos la lista para que el Spline solo reciba puntos donde el pedal está presionado.
        // Usamos Select para mantener el índice (posición X) pero ponemos valores "fuera de rango" 
        // o procesamos segmentos separados.
    
        // Si tu SplineHelper no soporta huecos, la forma más limpia es esta:
        var clutchSamples = _vm.Samples
            .Select(s => s.Clutch > 0.001f ? s.Clutch : -1.0f) // Marcamos los ceros como -1
            .ToList();

        ClutchPath.Data = SplineHelper.CreateSmoothCurve(clutchSamples, w, h);
    }
    
    private void DrawGrid()
    {
        GraphCanvas.Children.Clear();
        
        GraphCanvas.ClipToBounds = true;

        double w = GraphCanvas.ActualWidth;
        double h = GraphCanvas.ActualHeight;

        int horizontalLines = 4; // número de divisiones (0%,25%,50%,75%,100%)

        // Evitamos la primera y última línea
        for (int i = 1; i < horizontalLines; i++)
        {
            double y = h - i * (h / horizontalLines);
            var line = new Line()
            {
                X1 = 0,
                Y1 = y,
                X2 = w,
                Y2 = y,
                Stroke = Brushes.White,
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection { 2, 2 }
            };
            GraphCanvas.Children.Add(line);
        }

        // Luego agregas tus Path’s encima
        GraphCanvas.Children.Add(ThrottlePath);
        GraphCanvas.Children.Add(BrakePath);
        GraphCanvas.Children.Add(ClutchPath);
    }
}
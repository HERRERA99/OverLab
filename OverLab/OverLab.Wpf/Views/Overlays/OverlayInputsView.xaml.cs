using System.Windows;
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
        double w = ActualWidth;
        double h = ActualHeight;

        // Dibujamos acelerador y freno normalmente
        ThrottlePath.Data = SplineHelper.CreateSmoothCurve(_vm.Samples.Select(s => s.Throttle).ToList(), w, h);
        BrakePath.Data = SplineHelper.CreateSmoothCurve(_vm.Samples.Select(s => s.Brake).ToList(), w, h);

        // Lógica para el embrague
        var clutchValues = _vm.Samples.Select(s => s.Clutch).ToList();
    
        // Si el valor máximo es casi 0 (margen de error para sensores), ocultamos el path
        if (clutchValues.Max() < 0.01f) 
        {
            ClutchPath.Visibility = Visibility.Collapsed;
        }
        else 
        {
            ClutchPath.Visibility = Visibility.Visible;
            ClutchPath.Data = SplineHelper.CreateSmoothCurve(clutchValues, w, h);
        }
    }
    
    private void DrawGrid()
    {
        GraphCanvas.Children.Clear();

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
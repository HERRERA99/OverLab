using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using OverLab.Core.App;
using OverLab.Core.Telemetry;
using OverLab.Core.Widgets;

namespace OverLab.Wpf.ViewModels
{
    public class OverlayInputsViewModel : INotifyPropertyChanged, IViewModel
    {
        private readonly OverlayController _controller;
        private readonly OverlayId _overlayId;

        public ObservableCollection<float> ThrottleValues { get; } = new();
        public ObservableCollection<float> BrakeValues { get; } = new();
        public ObservableCollection<float> ClutchValues { get; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool IsVisible => _controller.GetOverlay(_overlayId).IsVisible;
        
        public IReadOnlyList<PedalSample> Samples { get; private set; } = Array.Empty<PedalSample>();

        public OverlayInputsViewModel(
            OverlayController controller,
            OverlayId overlayId,
            IPedalTraceWidget pedalWidget)
        {
            _controller = controller;
            _overlayId = overlayId;

            // Reactivo a datos
            pedalWidget.SamplesUpdated += OnSamplesUpdated;

            // Reactivo a visibilidad
            _controller.OverlayVisibilityChanged += id =>
            {
                if (id == _overlayId)
                {
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsVisible))
                    );
                }
            };

        }

        private void OnSamplesUpdated(IReadOnlyList<PedalSample> samples)
        {
            Samples = samples;

            // Notificamos UNA sola vez
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(Samples))
            );
        }

    }
}
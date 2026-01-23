using System;
using System.ComponentModel;
using OverLab.Core.App;
using OverLab.Core.Telemetry;
using OverLab.Core.Widgets;

namespace OverLab.Wpf.ViewModels
{
    public class OverlayInputsViewModel : INotifyPropertyChanged, IViewModel
    {
        private readonly OverlayController _controller;
        private readonly OverlayId _overlayId;

        public event PropertyChangedEventHandler? PropertyChanged;

        // =========================
        // VISIBILIDAD
        // =========================

        public bool IsVisible =>
            _controller.GetOverlay(_overlayId).IsVisible;

        // =========================
        // DRAG ENABLED
        // =========================

        private bool _isDragEnabled;
        public bool IsDragEnabled
        {
            get => _isDragEnabled;
            set
            {
                if (_isDragEnabled == value)
                    return;

                _isDragEnabled = value;
                OnPropertyChanged(nameof(IsDragEnabled));
            }
        }

        // =========================
        // DATOS
        // =========================

        public IReadOnlyList<PedalSample> Samples { get; private set; }
            = Array.Empty<PedalSample>();

        // =========================
        // CONFIGURACIÓN OVERLAY
        // =========================

        public OverlayConfiguration Configuration =>
            _controller.GetOverlay(_overlayId);

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
                    OnPropertyChanged(nameof(IsVisible));

                    // Opcional: desactivar drag al ocultar
                    if (!IsVisible)
                        IsDragEnabled = false;
                }
            };
        }

        private void OnSamplesUpdated(IReadOnlyList<PedalSample> samples)
        {
            Samples = samples;
            OnPropertyChanged(nameof(Samples));
        }

        // =========================
        // HELPERS
        // =========================

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName)
            );
        }
    }
}

using System.Windows;
using OverLab.Core.App;
using OverLab.Core.Telemetry;
using OverLab.Core.Widgets;
using OverLab.Providers.iRacing;
using OverLab.Wpf.ViewModels;
using OverLab.Wpf.Views.Overlays;

namespace OverLab.Wpf
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var overlayController = new OverlayController();
            overlayController.SetOverlayVisible(OverlayId.Pedals, false);

            // Widget
            PedalTraceWidget pedalWidget = new PedalTraceWidget();

            // Telemetry
            var telemetryProvider = new IracingTelemetryProvider();
            var telemetryManager = new TelemetryManager(
                telemetryProvider,
                pedalWidget);
            telemetryManager.Start();

            // ViewModel
            var pedalOverlayVm = new OverlayInputsViewModel(
                overlayController,
                OverlayId.Pedals,
                pedalWidget);

            // View
            var pedalOverlayView = new OverlayInputsView(pedalOverlayVm);

            overlayController.OverlayVisibilityChanged += id =>
            {
                if (id != OverlayId.Pedals) return;

                Dispatcher.Invoke(() =>
                {
                    if (overlayController.GetOverlay(id).IsVisible)
                        pedalOverlayView.Show();
                    else
                        pedalOverlayView.Hide();
                });
            };

            // Main window
            var mainWindow = new MainWindow
            {
                DataContext = new MainPageViewModel(overlayController)
            };
            mainWindow.Show();
        }
    }
}
using OverLab.Providers.Abstractions;
using OverLab.Providers.Models;
using iRacingSdkWrapper;

namespace OverLab.Providers.iRacing;

public class IracingTelemetryProvider : ITelemetryProvider
{
    public event Action<PedalTelemetry>? PedalTelemetryUpdated;

    private readonly SdkWrapper _wrapper;

    public IracingTelemetryProvider()
    {
        _wrapper = new SdkWrapper();

        // Puedes ajustar esto para pruebas (30 / 60)
        _wrapper.TelemetryUpdateFrequency = 60;

        _wrapper.TelemetryUpdated += OnTelemetryUpdated;
    }

    private void OnTelemetryUpdated(object? sender, SdkWrapper.TelemetryUpdatedEventArgs e)
    {
        float throttle = e.TelemetryInfo.Throttle.Value;
        float brake    = e.TelemetryInfo.Brake.Value;
    
        // INVERSIÓN AQUÍ: 1.0 es suelto, 0.0 es pisado. 
        // Al restar (1 - valor), 1.0 se convierte en 0 (línea abajo).
        float clutchRaw = e.TelemetryInfo.Clutch.Value;
        float clutch = 1.0f - clutchRaw; 

        PedalTelemetryUpdated?.Invoke(new PedalTelemetry
        {
            Throttle = throttle,
            Brake = brake,
            Clutch = clutch
        });
    }

    public void Start() => _wrapper.Start();
    public void Stop()  => _wrapper.Stop();
}
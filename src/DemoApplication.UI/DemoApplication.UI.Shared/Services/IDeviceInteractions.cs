namespace DemoApplication.UI.Shared.Services;

public interface IDeviceInteractions
{
    bool VibrationSupported();
    void VibrateDevice();
    bool CanGetPowerSource();
    string GetPowerSource();
    Task<string> GetLastKnownLocation();
}
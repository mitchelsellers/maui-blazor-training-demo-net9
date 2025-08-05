using DemoApplication.UI.Shared.Services;

namespace DemoApplication.UI.Web.Services;

public class DeviceInteractions : IDeviceInteractions
{
    public bool VibrationSupported()
    {
        return false;
    }

    public void VibrateDevice()
    {
        throw new NotImplementedException();
    }

    public bool CanGetPowerSource()
    {
        return false;
    }

    public string GetPowerSource()
    {
        throw new NotImplementedException();
    }

    public Task<string> GetLastKnownLocation()
    {
        // Could do this with JS?
        throw new NotImplementedException();
    }
}
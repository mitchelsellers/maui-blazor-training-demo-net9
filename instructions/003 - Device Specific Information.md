## Device Specific Interactions

Interacting with device specific elements are the first run great examples!  Lets dive in

### 1. Create an Interface for Differing Implementations

Within `DemoApplication.UI.Shared/Services` add an interface `IDeviceInteractions.cs` witht he following content.

```` csharp
public interface IDeviceInteractions
{
    bool VibrationSupported();
    void VibrateDevice();
    bool CanGetPowerSource();
    string GetPowerSource();
    string GetLastKnownLocation();
}
````

### 2. Create an Implementation for MAUI

Within `DemoApplication.UI/Services` add a class `DeviceInteractions.cs` with the following content

```` csharp 
using DemoApplication.UI.Shared.Services;

namespace DemoApplication.UI.Services;

public class DeviceInteractions : IDeviceInteractions
{
    public bool VibrationSupported()
    {
        //https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/device/vibrate?view=net-maui-8.0&tabs=windows
        return Vibration.Default.IsSupported;
    }

    public void VibrateDevice()
    {
        if (VibrationSupported())
            Vibration.Default.Vibrate(TimeSpan.FromSeconds(3));
        else
            throw new NotSupportedException("Vibration is not supported on this device.");
    }

    public bool CanGetPowerSource()
    {
        return true;
    }

    public string GetPowerSource()
    {
        //https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/device/battery?view=net-maui-8.0&tabs=windows
        return Battery.Default.PowerSource switch
        {
            BatteryPowerSource.Wireless => "Wireless charging",
            BatteryPowerSource.Usb => "USB cable charging",
            BatteryPowerSource.AC => "Device is plugged in to a power source",
            BatteryPowerSource.Battery => "Device isn't charging",
            _ => "Unknown"
        };
    }

    public async Task<string> GetLastKnownLocation()
    {
        // Documentation: https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/device/geolocation?view=net-maui-8.0&tabs=android
        try
        {
            var location = await Geolocation.Default.GetLastKnownLocationAsync();

            if (location != null)
                return $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            // Handle not supported on device exception
            return "Not Supported";
        }
        catch (FeatureNotEnabledException fneEx)
        {
            // Handle not enabled on device exception
            return "Not Enabled";
        }
        catch (PermissionException pEx)
        {
            // Handle permission exception
            return "No Permission";
        }
        catch (Exception ex)
        {
            // Unable to get location
            return $"Error: {ex.Message}";
        }

        return "Unknown";
    }
}
````

After doing this add the following line to the `MauiProgram.cs` file to register the injection

```` csharp
builder.Services.AddTransient<IDeviceInteractions, DeviceInteractions>();
````

### 3. Create an Implementation for Web

Within the Wb project create `DeviceInteractions.cs` with the following content

```` csharp
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
````

And register it as well in the `Program.cs`

```` csharp
builder.Services.AddTransient<IDeviceInteractions, DeviceInteractions>();
````

### Create a Sample Page for Demo

Create a new Razor Compeonent in the Shared project under the `/Pages` folder named `DeviceAccess.razor` with the below content.

```` razor
@page "/DeviceAccess"
@using DemoApplication.UI.Shared.Services
@inject IDeviceInteractions DeviceInteractions
@inject IJSRuntime JSRuntime

<PageTitle>Device Features</PageTitle>

<h4>Device Specific Features</h4>

<p>
	<button class="btn btn-primary" @onclick="TryVibrateDevice" disabled="@(!DeviceInteractions.VibrationSupported())">
		Try Vibrate Device
	</button>
</p>

<p>
	<button class="btn btn-primary" @onclick="TryCachedLocation">Try Cached Location (Last Known: @LastKnownLocation)</button>
</p>

<p>
	<button class="btn btn-primary" @onclick="GetPowerSource" disabled="@(!DeviceInteractions.CanGetPowerSource())">
		Get Power Source Information: @PowerSource)
	</button>
</p>

@code {

    public string LastKnownLocation { get; set; }
    public string PowerSource { get; set; } = "Not Checked";

    private async Task TryVibrateDevice()
    {
	    if (DeviceInteractions.VibrationSupported())
	    {
            DeviceInteractions.VibrateDevice();
	    }
        else
        {
            await JSRuntime.InvokeVoidAsync("alert", "Vibration is not supported");
        }
    }

    private async Task TryCachedLocation()
    {
		LastKnownLocation = await DeviceInteractions.GetLastKnownLocation();
    }

    private void GetPowerSource()
    {
		PowerSource = DeviceInteractions.GetPowerSource();
    }
}
````


### 5. Add Menu Link

Update `NavMenu.razor` to include a new link.

```` razor
<div class="nav-item px-3">
    <NavLink class="nav-link" href="DeviceAccess">
        <span class="bi bi-list-nested-nav-menu" aria-hidden="true"></span> Device Access
    </NavLink>
</div>
````

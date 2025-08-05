## Demo with Authentication

The goal of this process is to explore a different sample/architecture for the hybrid application experience.  For this to work properly you must have.

* SQL Server running locally (LocalDB or fixed blank local DB)
* Internet Access to download the sample.

### Instructions & Overview

The sample reviewed is actually published by Microsoft here - https://learn.microsoft.com/en-us/aspnet/core/blazor/hybrid/security/maui-blazor-web-identity?view=aspnetcore-9.0

### Downloading

Download the sample from here = https://github.com/dotnet/blazor-samples

You are going to have to download the full repo using the `Code` Download ZIP option

Find the folder `9.0/MauiBlazorWebIdentity` and copy its contents, for the demonstration we are going to place it in the /src folder of this repo.

### Launching

Instructions pulled from Microsoft

1. Start the MauiBlazorWeb.Web project without debugging. In Visual Studio, right-click on the project and select Debug > Start without Debugging.
2. Inspect the Identity endpoints by navigating to https://localhost:7157/swagger in a browser.
3. Navigate to https://localhost:7157/account/register to register a user in the Blazor Web App. Immediately after the user is registered, use the Click here to confirm your account link in the UI to confirm the user's email address because a real email sender isn't registered for account confirmation.
4. Start (F5) the MauiBlazorWeb MAUI project. You can set the debug target to either Windows or an Android emulator.
5. Notice you can only see the Home and Login pages.
6. Log in with the user that you registered.
7. Notice you can now see the shared Counter and Weather pages.
8. Log out and notice you can only see the Home and Login pages again.
9. Navigate to https://localhost:7157/ and the web app behaves the same.

### Lets Explore!
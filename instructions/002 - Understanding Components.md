## Understanding Components

The goal of these steps are to understand what a component is, the basic of interacting with them, and managing C# -> JS and JS -> C# Interactions.

### 1. Creating A Basic Component

Within `DemoApplication.UI.Shared` add a new Razor Component to the root named `SimpleAlert.razor`

```` razor
<div class="alert alert-info">
	@Title
</div>

@code {

	[Parameter]
	public string Title { get; set; } = null!;

}
````

This is an example of a specific named parameter, you can use this by updating the `Home.razor` by adding the following just after the `<h1>`

```` razor
<SimpleAlert Title="Testing"></SimpleAlert>
````

### 2. Creating a More Usable Component - With RenderFragment

Within the same project create a new Razor Component `Alert.razor` with the below content

```` razor
<div class="alert alert-info">
	@ChildContent
</div>

@code
{
	[Parameter]
	public RenderFragment? ChildContent { get; set; }
}
````

Using this one, the alert can have a fragment of HTML, or even other components within and it will be rendered.  You can update the homepage to put the information about the current factor and platform in this elements such as.

```` razor
<Alert>
	Welcome to your new app running on <em>@factor</em> using <em>@platform</em>.
</Alert>
````

### 3. Setup for JS Interop

We have two primary launch points, so we have two edits.

Add the following script block to the end of the `<body>` tag on the following files:

* `DemoApplication.UI/wwwroot/index.html`
* `DemoApplication.UI.Web/Components/App.razor`

```` html
<script>
    function demoMethod() {
        alert('This is from JS!');
    }

    function showPrompt(message) {
        return prompt(message, 'Type anything here');
    }
</script>
````

This isn an example, but note you can get access to ANY JS that you need, this could be third-party or otherwise.

### 4. Actually Use the Calls

Add a new Razor Component in `DemoApplication.UI.Shared/Pages` called `InteropExample.razor` with the following content

```` razor
@page "/InteropExample"

@inject IJSRuntime JSRuntime

<PageTitle>Interop Example</PageTitle>

<h1>Interop Example</h1>

<p>
	<button class="btn btn-primary" @onclick="TestJsInterop">Test JS Interop</button>
</p>

<p>
	<button class="btn btn-primary" @onclick="TestJsInteropParameters">Test JS Interop w/ Parameters</button>
</p>

<p>
	<button class="btn btn-primary" @onclick="TestJsInteropReceive">Test JS Interop w/ Return Value</button>
</p>

@if (!string.IsNullOrEmpty(FromJavascript))
{
	<p>The following was received from JS <strong>'@FromJavascript'</strong></p>
}


@code {

	public string FromJavascript { get; set; } = null!;

	private async Task TestJsInterop()
	{
		await JSRuntime.InvokeVoidAsync("demoMethod");
	}

	private async Task TestJsInteropParameters()
	{
		await JSRuntime.InvokeVoidAsync("alert", "I can even pass stuff!");
	}

	private async Task TestJsInteropReceive()
	{
		FromJavascript = await JSRuntime.InvokeAsync<string>("showPrompt", "Please provide a value to display");
	}
}
````

After doing this, pdate the `NavMenu` under the `Layout` folder to include a new link to this item using the below markup.

```` razor
<div class="nav-item px-3">
    <NavLink class="nav-link" href="InteropExample">
        <span class="bi bi-list-nested-nav-menu" aria-hidden="true"></span> Interop Example
    </NavLink>
</div>
````


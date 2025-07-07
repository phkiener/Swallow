[![Swallow.Blazor.Reactive](https://img.shields.io/nuget/v/Swallow.Blazor.Reactive?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/Swallow.Blazor.Reactive/)
&nbsp;
![Library](https://img.shields.io/badge/c%23-library-blue?style=for-the-badge)
&nbsp;
![MIT license](https://img.shields.io/badge/license-mit-brightgreen?style=for-the-badge)

---

# Interactivity - statically rendered

When working with Blazor, you've got a choice between three options:

1. Statically render everything, using clientside JS to allow interactive behavior
2. Use Blazor Server and open a persistent websocket connection
3. Compile your app to WebAssembly and use Blazor WebAssembly to host your app on the client

All of these have drawbacks, as usual. Static rendering is very neat, but the lack of interactivity
is a problem - which usually ends in either a websocket or WASM being used. After all, if you could
write some JS to get the job done, why even bother with Blazor in the first place? That's not what
we're here for.

## Introducing: `Swallow.Blazor.Reactive`

Using the power of [HTMX](https://htmx.org) (for now), you can host your completely statically
rendered app and have interactivity added via discrete requests and responses, eliminating the
need for websockets or extensive clientside JS. Though this, as well, comes with a drawback, or
rather a caveat: It's not fit for high-frequency interactions. While it's no problem to register a
listener for `mousemove` in Blazor WebAssembly, it does become a problem when you're sending web
requests every time the mouse is being moved.

## Setup

First, setup the routing to reactive components in your `Program.cs`:
```csharp
var builder = WebApplication.CreateBuilder();
builder.Services.AddReactiveRendering(); // <- this part!

var app = builder.Build();

app.MapRazorComponents<App>();
app.MapReactiveComponents(typeof(App).Assembly); // <- this part!
app.MapRoutedComponents(typeof(App).Assembly); // <- and/or this part!

await app.RunAsync();
```

Add `[ReactiveComponent]` or `[RoutedComponent]` to your components:
```csharp
[RoutedComponent("reactive/some-component")]
public sealed class SomeComponent : ComponentBase
{
    // ...
}

[ReactiveComponent()]
public sealed class SomeReactiveComponent : ComponentBase
{
    // ...
}
```

So... what does that mean?

### Routed components

While you can render full pages using `@page "/some/path"`, this will always include the full HTML
body; the `<head>`, your layout and all the other content. Routed components will be rendered
*directly*, without anything else going on. You can use this do integrate HTMX to your workflow:

```csharp
@using Swallow.Blazor.Reactive.Abstractions
@attribute [RoutedComponent("current-time")]

<div>
    <span>It's currently @DateTime.Now.ToString("O")</span>
    <button type="button" hx-get="/current-time" hx-target="closest div" hx-swap="outerHTML">Refresh</button>
</div>
```

Clicking on the button will re-render the outer div of that component by requesting only that single
component from the server. Neat!

### Reactive components

While working with HTMX is quite comfortable, we can go even one step further.

```csharp
@using Swallow.Blazor.Reactive.Abstractions
@attribute [ReactiveComponent]
@inject IReactiveStateHandler State

<div>
    <span>It's currently @DateTime.Now.ToString("O")</span>
    <button type="button" rx-id="refresh" @onclick="@(() => count += 1)">Refresh</button>
</div>

@code {
    private int count = 0;

    [CascadingParameter]
    public required IReactiveIsland Island { get; set;}

    protected override OnInitialized()
    {
        State.Register(Island, nameof(count), () => count);
    }
}
```

Clicking on the button will rerender that component as before, but now the state of `counter` will
be kept across requests!

For Blazor Server or Blazor WebAssembly, you'd annotate the component with an `@rendermode`
attribute... but that doesn't work for this case, sadly. Instead, to start a "reactive island",
you'll need to use `<ReactiveBoundary>`:

```csharp
<ReactiveBoundary Name="Time" ComponentType="@typeof(InteractiveTime)" />
```

## About using HTMX

Right now, `Swallow.Blazor.Reactive` uses [HTMX](https://htmx.org) to facilitate sending requests
triggered by clientside events like `click` or `change`. All HTMX attributes are available for your
custom use, but I might switch to a custom implementation along the way.

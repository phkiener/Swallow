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

## Setup

...

## Usage

..

## Introducing: `Swallow.Blazor.Reactive`

Using the power of [HTMX](https://htmx.org) (for now), you can host your completely statically
rendered app and have interactivity added via discrete requests and responses, eliminating the
need for websockets or extensive clientside JS. Though this, as well, comes with a drawback, or
rather a caveat: It's not fit for high-frequency interactions. While it's no problem to register a
listener for `mousemove` in Blazor WebAssembly, it does become a problem when you're sending web
requests every time the mouse is being moved.

## About using HTMX

Right now, `Swallow.Blazor.Reactive` uses [HTMX](https://htmx.org) to facilitate sending requests
triggered by clientside events like `click` or `change`. This may change in the future, so the use
of HTMX is abstracted away; instead of `hx-*` attribute, custom `rx-*` attributes should be used.
These will automatically be translated to `hx-*` attributes as needed.

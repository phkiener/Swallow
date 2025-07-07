using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web.HtmlRendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Swallow.Blazor.Reactive.Abstractions.State;

namespace Swallow.Blazor.Reactive.Rendering;

internal sealed class StatefulReactiveComponentInvoker(StatefulReactiveComponentRenderer renderer, IReactiveStateHandler stateHandler)
{
    public Task Render(HttpContext context, string identifier, Type componentType)
    {
        return renderer.Dispatcher.InvokeAsync(() => RenderComponentCore(context, identifier, componentType));
    }

    private async Task RenderComponentCore(HttpContext context, string identifier, Type componentType)
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        await InitializeStandardComponentServicesAsync(context);

        var trigger = context.Request.Headers["rx-trigger"].ToString();
        var eventName = context.Request.Headers["rx-event"].ToString();

        HtmlRootComponent rootComponent;
        try
        {
            var parameters = new Dictionary<string, object?>
            {
                [nameof(StatefulReactiveComponentRoot.HttpContext)] = context,
                [nameof(StatefulReactiveComponentRoot.Island)] = new ReactiveIsland(identifier),
                [nameof(StatefulReactiveComponentRoot.ComponentType)] = componentType
            };

            rootComponent = renderer.BeginRenderingComponent(typeof(StatefulReactiveComponentRoot), ParameterView.FromDictionary(parameters));

            if (trigger is not (null or ""))
            {
                var eventBody = context.Request.Form["__event"].ToString();
                await renderer.DispatchEventAsync(trigger, eventName, eventBody);
            }

            await rootComponent.QuiescenceTask;

        }
        catch (NavigationException e)
        {
            context.Response.Redirect(e.Location);
            return;
        }

        await using var writer = new HttpResponseStreamWriter(context.Response.Body, Encoding.UTF8);
        rootComponent.WriteHtmlTo(writer);

        if (stateHandler is IReactiveStateProvider stateProvider)
        {
            foreach (var (key, value) in stateProvider.Collect())
            {
                await writer.WriteLineAsync($"<input type=\"hidden\" name=\"{key}\" value=\"{value}\" rx-state>");
            }
        }

        await writer.FlushAsync();
    }

    // Partially copied from Microsoft.AspNetCore.Components.Endpoints.EndpointHtmlRenderer
    // See https://github.com/dotnet/dotnet/blob/1b0c85e32e038b5751036b44fe4c037e5bf66b04/src/aspnetcore/src/Components/Endpoints/src/Rendering/EndpointHtmlRenderer.cs
    private static async Task InitializeStandardComponentServicesAsync(HttpContext httpContext)
    {
        var navigationManager = httpContext.RequestServices.GetRequiredService<NavigationManager>();
        if (navigationManager is IHostEnvironmentNavigationManager hostEnvironmentNavigationManager)
        {
            hostEnvironmentNavigationManager.Initialize(GetContextBaseUri(httpContext.Request), GetFullUri(httpContext.Request));
        }

        var authenticationStateProvider = httpContext.RequestServices.GetService<AuthenticationStateProvider>();
        if (authenticationStateProvider is IHostEnvironmentAuthenticationStateProvider hostEnvironmentAuthenticationStateProvider)
        {
            var authenticationState = new AuthenticationState(httpContext.User);
            hostEnvironmentAuthenticationStateProvider.SetAuthenticationState(Task.FromResult(authenticationState));
        }

        if (authenticationStateProvider != null)
        {
            var authStateListeners = httpContext.RequestServices.GetServices<IHostEnvironmentAuthenticationStateProvider>();
            Task<AuthenticationState>? authStateTask = null;
            foreach (var authStateListener in authStateListeners)
            {
                authStateTask ??= authenticationStateProvider.GetAuthenticationStateAsync();
                authStateListener.SetAuthenticationState(authStateTask);
            }
        }

        var stateHandler = httpContext.RequestServices.GetRequiredService<IReactiveStateHandler>();
        if (stateHandler is IReactiveStateProvider initializableReactiveStateHandler)
        {
            var formCollection = await httpContext.Request.ReadFormAsync();
            var formData = formCollection.ToDictionary(static kvp => kvp.Key, static kvp => kvp.Value.ToString());
            initializableReactiveStateHandler.Initialize(formData);
        }
    }

    private static string GetFullUri(HttpRequest request)
    {
        return UriHelper.BuildAbsolute(request.Scheme, request.Host, request.PathBase, request.Path, request.QueryString);
    }

    private static string GetContextBaseUri(HttpRequest request)
    {
        var result = UriHelper.BuildAbsolute(request.Scheme, request.Host, request.PathBase);

        return result.EndsWith('/') ? result : result + "/";
    }
}

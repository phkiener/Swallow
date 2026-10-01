using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using RouteData = Microsoft.AspNetCore.Components.RouteData;

namespace Swallow.SiteGen;

public static class Serve
{
    public static async Task RunAsync(string listenUrl)
    {
        var builder = WebApplication.CreateBuilder();
        builder.Logging.AddFilter("Swallow.SiteGen", static l => l >= LogLevel.Information);
        builder.Logging.AddFilter("Microsoft.Hosting.Lifetime", static l => l >= LogLevel.Information);
        builder.Logging.AddFilter(static l => l >= LogLevel.Warning);
        builder.Services.AddRazorComponents();
        builder.Services.AddSingleton<SourceCacheContext>();
        builder.Services.AddSingleton<SourceRepository>(static _ => Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json"));

        StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

        var host = builder.Build();
        host.MapStaticAssets();
        host.MapRazorComponents<RootComponent>().DisableAntiforgery();

        await host.RunAsync(listenUrl);
    }

    private sealed class RootComponent : ComponentBase
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<Router>(0);
            builder.AddComponentParameter(1, nameof(Router.AppAssembly), typeof(Serve).Assembly);
            builder.AddComponentParameter(2, nameof(Router.Found), (RenderFragment<RouteData>)RenderFoundComponent);
            builder.CloseComponent();
            base.BuildRenderTree(builder);
        }

        private static RenderFragment RenderFoundComponent(RouteData routeData)
        {
            return builder =>
            {
                builder.OpenComponent<RouteView>(0);
                builder.AddComponentParameter(1, nameof(RouteView.RouteData), routeData);
                builder.CloseComponent();
            };
        }
    }
}

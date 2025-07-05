using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components.Endpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Swallow.Blazor.Reactive.Abstractions;

namespace Swallow.Blazor.Reactive.DataSource;

internal sealed class ReactiveComponentEndpointDataSource : EndpointDataSource
{
    private readonly Lock lockObject = new();
    private readonly List<Assembly> assemblies = [];

    private List<Endpoint>? endpoints;
    private CancellationTokenSource changeTokenSource;
    private IChangeToken changeToken;

    internal ReactiveComponentEndpointDataSource()
    {
        GenerateChangeToken();
    }

    public override IChangeToken GetChangeToken() => changeToken;

    public override IReadOnlyList<Endpoint> Endpoints
    {
        get
        {
            if (endpoints is null)
            {
                BuildEndpoints();
                GenerateChangeToken();
            }

            return endpoints;
        }
    }

    internal void Include(Assembly assembly)
    {
        lock (lockObject)
        {
            assemblies.Add(assembly);
        }

        BuildEndpoints();
        GenerateChangeToken();
    }

    [MemberNotNull(nameof(endpoints))]
    private void BuildEndpoints()
    {
        var foundEndpoints = new List<Endpoint>();
        lock (lockObject)
        {
            var components = assemblies.SelectMany(static a => a.GetExportedTypes())
                .Where(static t => t.GetCustomAttributes<ReactiveComponentAttribute>().Any())
                .ToList();

            foreach (var component in components)
            {
                var attributes = component.GetCustomAttributes<ReactiveComponentAttribute>();
                foreach (var attribute in attributes)
                {
                    var endpointBuilder = new RouteEndpointBuilder(
                        requestDelegate: RenderReactiveComponent,
                        routePattern: RoutePatternFactory.Parse(attribute.RouteTemplate),
                        order: 0);

                    endpointBuilder.DisplayName = $"{endpointBuilder.RoutePattern.RawText} ({component.Name})";
                    endpointBuilder.Metadata.Add(attribute);
                    endpointBuilder.Metadata.Add(new HttpMethodMetadata(attribute.Methods));

                    // Normally, mapped components include ComponentTypeMetadata
                    // These configure route values so that the <Router> can take over when rendering the root component.
                    // We don't need or want that, so we instead apply ReactiveComponentTypeMetadata which our custom
                    // root component will use to render the correct component.
                    endpointBuilder.Metadata.Add(new ReactiveComponentTypeMetadata(component));
                    endpointBuilder.Metadata.Add(new RootComponentMetadata(typeof(ReactiveComponentRoot)));

                    foreach (var componentAttribute in component.GetCustomAttributes())
                    {
                        if (componentAttribute is ReactiveComponentAttribute or RequiredMemberAttribute)
                        {
                            continue;
                        }

                        endpointBuilder.Metadata.Add(componentAttribute);
                    }

                    // TODO: Support conventions

                    var endpoint = endpointBuilder.Build();
                    foundEndpoints.Add(endpoint);
                }
            }
        }

        endpoints = foundEndpoints;
    }

    private static async Task RenderReactiveComponent(HttpContext httpContext)
    {
        var isReactive = httpContext.Request.Headers.ContainsKey("rx-request");
        if (!isReactive)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            await httpContext.Response.CompleteAsync();
        }
        else
        {
            var invoker = httpContext.RequestServices.GetRequiredService<IRazorComponentEndpointInvoker>();
            await invoker.Render(httpContext);
        }
    }

    [MemberNotNull(nameof(changeTokenSource))]
    [MemberNotNull(nameof(changeToken))]
    private void GenerateChangeToken()
    {
        var previousChangeTokenSource = changeTokenSource;
        changeTokenSource = new CancellationTokenSource();
        changeToken = new CancellationChangeToken(changeTokenSource.Token);

        previousChangeTokenSource?.Cancel();
        previousChangeTokenSource?.Dispose();
    }
}

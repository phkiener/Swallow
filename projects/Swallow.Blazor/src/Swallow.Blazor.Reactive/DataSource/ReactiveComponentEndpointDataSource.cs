using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Endpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Swallow.Blazor.Reactive.Abstractions;
using Swallow.Blazor.Reactive.Rendering;

namespace Swallow.Blazor.Reactive.DataSource;

internal sealed class ReactiveComponentEndpointDataSource : EndpointDataSource
{
    private static readonly object? RenderModesMetadata = BuildRenderModeMetadata();
    private readonly Lock lockObject = new();
    private readonly List<Assembly> assemblies = [];
    private readonly List<Action<EndpointBuilder>> conventions = [];
    private readonly List<Action<EndpointBuilder>> finallyConventions = [];

    private List<Endpoint>? endpoints;
    private CancellationTokenSource changeTokenSource;
    private IChangeToken changeToken;

    internal ReactiveComponentEndpointDataSource()
    {
        ConventionBuilder = new EndpointConventionBuilder(this);
        GenerateChangeToken();
    }

    public IEndpointConventionBuilder ConventionBuilder { get; }

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
            BuildReactiveComponentEndpoints(foundEndpoints);
            BuildStatefulReactiveComponentEndpoints(foundEndpoints);
        }

        endpoints = foundEndpoints;
    }

    private void BuildReactiveComponentEndpoints(List<Endpoint> target)
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
                endpointBuilder.Metadata.Add(new ComponentTypeMetadata(component));
                endpointBuilder.Metadata.Add(new RootComponentMetadata(typeof(ReactiveComponentRoot)));

                if (RenderModesMetadata is not null)
                {
                    endpointBuilder.Metadata.Add(RenderModesMetadata);
                }

                FinalizeEndpoint(component: component, endpointBuilder: endpointBuilder);

                var endpoint = endpointBuilder.Build();
                target.Add(endpoint);
            }
        }
    }

    private void BuildStatefulReactiveComponentEndpoints(List<Endpoint> target)
    {
        var components = assemblies.SelectMany(static a => a.GetExportedTypes())
            .Where(static t => t.GetCustomAttribute<StatefulReactiveComponentAttribute>() is not null)
            .ToList();

        foreach (var component in components)
        {
            var attribute = component.GetCustomAttribute<StatefulReactiveComponentAttribute>()!;
            var routeTemplate = attribute.RouteTemplate ?? $"_rx/{component.Assembly.GetName().Name}/{component.FullName}";
            var routePattern = RoutePatternFactory.Parse(routeTemplate);

            if (routePattern.Parameters.Any())
            {
                throw new InvalidOperationException($"Route templates for stateful reactive components must not contain parameters.");
            }

            var endpointBuilder = new RouteEndpointBuilder(
                requestDelegate: RenderStatefulReactiveComponent,
                routePattern: routePattern,
                order: 0);

            endpointBuilder.DisplayName = $"{endpointBuilder.RoutePattern.RawText} ({component.Name})";
            endpointBuilder.Metadata.Add(attribute);
            endpointBuilder.Metadata.Add(new HttpMethodMetadata([HttpMethod.Post.Method]));
            endpointBuilder.Metadata.Add(new StatefulReactiveComponentMetadata(component));

            FinalizeEndpoint(component: component, endpointBuilder: endpointBuilder);

            var endpoint = endpointBuilder.Build();
            target.Add(endpoint);
        }
    }

    private void FinalizeEndpoint(Type component, RouteEndpointBuilder endpointBuilder)
    {
        foreach (var componentAttribute in component.GetCustomAttributes())
        {
            if (componentAttribute is ReactiveComponentAttribute or RequiredMemberAttribute)
            {
                continue;
            }

            endpointBuilder.Metadata.Add(componentAttribute);
        }

        foreach (var convention in conventions)
        {
            convention(endpointBuilder);
        }

        foreach (var finallyConvention in finallyConventions)
        {
            finallyConvention(endpointBuilder);
        }
    }

    private static Task RenderReactiveComponent(HttpContext httpContext)
    {
        var isReactive = httpContext.Request.Headers.ContainsKey("hx-request");
        if (!isReactive)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            return httpContext.Response.CompleteAsync();
        }

        var invoker = httpContext.RequestServices.GetRequiredService<IRazorComponentEndpointInvoker>();
        return invoker.Render(httpContext);
    }

    private static Task RenderStatefulReactiveComponent(HttpContext httpContext)
    {
        var isReactive = httpContext.Request.Headers.ContainsKey("hx-request");
        if (!isReactive)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            return httpContext.Response.CompleteAsync();
        }

        var invoker = httpContext.RequestServices.GetRequiredService<StatefulReactiveComponentInvoker>();
        return invoker.Render(httpContext);
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

    private static object? BuildRenderModeMetadata()
    {
        var definingAssembly = typeof(IRazorComponentEndpointInvoker).Assembly;
        var metadataType = definingAssembly.GetType("Microsoft.AspNetCore.Components.Endpoints.ConfiguredRenderModesMetadata");
        if (metadataType is null)
        {
            // Something has changed, but it's okay. The endpoint renderer will write (empty) state into a comment in the DOM...
            // useless and noisy but not an issue.
            return null;
        }

        return Activator.CreateInstance(metadataType, [Array.Empty<IComponentRenderMode>()]);
    }

    private sealed class EndpointConventionBuilder(ReactiveComponentEndpointDataSource dataSource) : IEndpointConventionBuilder
    {
        public void Add(Action<EndpointBuilder> convention)
        {
            lock (dataSource.lockObject)
            {
                dataSource.conventions.Add(convention);
            }
        }

        public void Finally(Action<EndpointBuilder> finallyConvention)
        {
            lock (dataSource.lockObject)
            {
                dataSource.finallyConventions.Add(finallyConvention);
            }
        }
    }
}

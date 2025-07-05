using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
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

                    var endpoint = endpointBuilder.Build();
                    foundEndpoints.Add(endpoint);
                }
            }
        }

        endpoints = foundEndpoints;
    }

    private static async Task RenderReactiveComponent(HttpContext httpContext)
    {
        var isReactive = httpContext.Request.Headers.ContainsKey("hx-request");
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

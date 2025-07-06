using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Routing;
using Swallow.Blazor.Reactive.Abstractions;
using Swallow.Blazor.Reactive.DataSource;

namespace Swallow.Blazor.Reactive.Components;

/// <summary>
/// A boundary for an reactive component that will hold its own state.
/// </summary>
public sealed partial class ReactiveBoundary(IEnumerable<EndpointDataSource> dataSources) : ComponentBase
{
    [CascadingParameter]
    private IReactiveIsland? ReactiveIsland { get; set; }

    private RouteEndpoint? targetRoute;
    private string? scriptSource;

    /// <summary>
    /// Name for this boundary, which will be used to generate an <see cref="IReactiveIsland"/>.
    /// </summary>
    [Parameter, EditorRequired]
    public required string Name { get; set; }

    /// <summary>
    /// Type of component to render inside the boundary.
    /// </summary>
    [Parameter, EditorRequired]
    public required Type ComponentType { get; set; }

    /// <summary>
    /// A nonce to render for the generated script, enabling you to use a content security policy.
    /// </summary>
    [Parameter]
    public string? ScriptNonce { get; set; }

    /// <summary>
    /// Additional HTML attributes to render on the outermost HTML element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public required IDictionary<string, object?>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (ReactiveIsland is not null)
        {
            throw new InvalidOperationException($"{nameof(ReactiveBoundary)} may not be nested within a reactive island.");
        }

        if (ComponentType.GetCustomAttribute<StatefulReactiveComponentAttribute>() is null)
        {
            throw new ArgumentException($"Only components marked with {nameof(StatefulReactiveComponentAttribute)} are supported.");
        }

        scriptSource = $"/{Assets["_content/Swallow.Blazor.Reactive/reactive.js"]}";
        targetRoute = FindEndpoint(dataSources, ComponentType);
        if (targetRoute is null)
        {
            throw new InvalidOperationException("No component endpoint found in data sources.");
        }
    }

    private static RouteEndpoint? FindEndpoint(IEnumerable<EndpointDataSource> dataSources, Type componentType)
    {
        var dataSourceQueue = new Queue<EndpointDataSource>(dataSources);
        while (dataSourceQueue.TryDequeue(out var dataSource))
        {
            if (dataSource is CompositeEndpointDataSource composite)
            {
                foreach (var innerDataSource in composite.DataSources)
                {
                    dataSourceQueue.Enqueue(innerDataSource);
                }
            }

            if (dataSource is ReactiveComponentEndpointDataSource)
            {
                var endpoint = dataSource.Endpoints
                    .OfType<RouteEndpoint>()
                    .FirstOrDefault(e => e.Metadata.GetMetadata<StatefulReactiveComponentMetadata>()?.ComponentType == componentType);

                if (endpoint is not null)
                {
                    return endpoint;
                }
            }
        }

        return null;
    }
}


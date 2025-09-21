using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Endpoints;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Swallow.Blazor.Reactive.Abstractions;
using Swallow.Blazor.Reactive.Abstractions.Rendering;
using Swallow.Blazor.Reactive.Abstractions.State;
using Swallow.Blazor.Reactive.Rendering;
using Swallow.Blazor.Reactive.Routing;

namespace Swallow.Blazor.Reactive.Components;

/// <summary>
/// A boundary for an reactive component that will hold its own state.
/// </summary>
public sealed partial class ReactiveBoundary(
    IEnumerable<EndpointDataSource> dataSources,
    IReactiveStateHandler stateHandler,
    TemplateBinderFactory templateBinderFactory) : ComponentBase, IDisposable
{
    private string? targetRoute;
    private string? scriptSource;
    private IReactiveStateProvider? stateProvider;
    private IReactiveIsland prerenderIsland = null!;

    [CascadingParameter]
    private IReactiveIsland? ReactiveIsland { get; set; }

    /// <summary>
    /// Name for this boundary, which will be used to generate an <see cref="IReactiveIsland"/>.
    /// </summary>
    [Parameter, EditorRequired]
    public required string Name { get; set; }

    /// <summary>
    /// Whether to prerender this component.
    /// </summary>
    [Parameter]
    public bool Prerender { get; set; } = false;

    /// <summary>
    /// Type of component to render inside the boundary.
    /// </summary>
    [Parameter, EditorRequired]
    public required Type ComponentType { get; set; }

    /// <summary>
    /// Components to pass to the rendered component.
    /// </summary>
    [Parameter]
    public Dictionary<string, object?>? ComponentParameters { get; set; }

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

        if (ComponentType.GetCustomAttribute<ReactiveComponentAttribute>() is null)
        {
            throw new ArgumentException($"Only components marked with {nameof(ReactiveComponentAttribute)} are supported.");
        }

        scriptSource = $"/{Assets["_content/Swallow.Blazor.Reactive/reactive.js"]}";
        var endpoint = FindEndpoint(dataSources, ComponentType);
        if (endpoint is null)
        {
            throw new InvalidOperationException("No component endpoint found in data sources.");
        }


        var binder = templateBinderFactory.Create(endpoint.RoutePattern);
        var routeValues = new RouteValueDictionary(ComponentParameters);
        targetRoute = binder.BindValues(routeValues);

        prerenderIsland = new ReactiveIsland(Name);
        if (stateHandler is IReactiveStateProvider provider)
        {
            stateProvider = provider;
            stateProvider.StateChanged += OnComponentStateChanged;
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

            if (dataSource is ReactiveComponentDataSource)
            {
                var endpoint = dataSource.Endpoints
                    .OfType<RouteEndpoint>()
                    .FirstOrDefault(e => e.Metadata.GetMetadata<ComponentTypeMetadata>()?.Type == componentType);

                if (endpoint is not null)
                {
                    return endpoint;
                }
            }
        }

        return null;
    }

    private void OnComponentStateChanged(object? sender, EventArgs eventArgs)
    {
        _ = InvokeAsync(StateHasChanged);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (stateProvider is not null)
        {
            stateProvider.StateChanged -= OnComponentStateChanged;
        }
    }
}

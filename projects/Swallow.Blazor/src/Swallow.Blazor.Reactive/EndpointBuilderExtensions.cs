using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Swallow.Blazor.Reactive.Abstractions;
using Swallow.Blazor.Reactive.DataSource;

namespace Swallow.Blazor.Reactive;

/// <summary>
/// Extensions on a <see cref="IEndpointRouteBuilder"/> to support reactive components.
/// </summary>
public static class EndpointBuilderExtensions
{
    /// <summary>
    /// Map all reactive components found in <c>Assembly.EntryAssembly()</c>.
    /// </summary>
    /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/> on which to add the components.</param>
    /// <exception cref="InvalidOperationException">When no entry assembly could be determined.</exception>
    public static void MapReactiveComponents(this IEndpointRouteBuilder endpoints)
    {
        var entryAssembly = Assembly.GetEntryAssembly()
                            ?? throw new InvalidOperationException("Could not determine entry assembly.");

        endpoints.MapReactiveComponents(entryAssembly);
    }

    /// <summary>
    /// Map all reactive components found in the given assemblies.
    /// </summary>
    /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/> on which to add the components.</param>
    /// <param name="assemblies">The assemblies in which to look for types with a <see cref="ReactiveComponentAttribute"/>.</param>
    public static void MapReactiveComponents(this IEndpointRouteBuilder endpoints, params Assembly[] assemblies)
    {
        var dataSource = endpoints.DataSources.OfType<ReactiveComponentEndpointDataSource>().FirstOrDefault();
        if (dataSource is null)
        {
            dataSource = new ReactiveComponentEndpointDataSource();
            endpoints.DataSources.Add(dataSource);
        }

        foreach (var assembly in assemblies)
        {
            dataSource.Include(assembly);
        }
    }
}

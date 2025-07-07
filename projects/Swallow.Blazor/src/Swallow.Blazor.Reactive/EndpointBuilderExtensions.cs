using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Swallow.Blazor.Reactive.Abstractions;
using Swallow.Blazor.Reactive.Routing;

namespace Swallow.Blazor.Reactive;

/// <summary>
/// Extensions on a <see cref="IEndpointRouteBuilder"/> to support reactive components.
/// </summary>
public static class EndpointBuilderExtensions
{
    /// <summary>
    /// Map all routed components found in <c>Assembly.EntryAssembly()</c>.
    /// </summary>
    /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/> on which to add the components.</param>
    /// <exception cref="InvalidOperationException">When no entry assembly could be determined.</exception>
    public static IEndpointConventionBuilder MapRoutedComponents(this IEndpointRouteBuilder endpoints)
    {
        var entryAssembly = Assembly.GetEntryAssembly()
                            ?? throw new InvalidOperationException("Could not determine entry assembly.");

        return endpoints.MapRoutedComponents(entryAssembly);
    }

    /// <summary>
    /// Map all reactive routed found in the given assemblies.
    /// </summary>
    /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/> on which to add the components.</param>
    /// <param name="assemblies">The assemblies in which to look for types with a <see cref="RoutedComponentAttribute"/>.</param>
    public static IEndpointConventionBuilder MapRoutedComponents(this IEndpointRouteBuilder endpoints, params Assembly[] assemblies)
    {
        var dataSource = GetOrCreateDataSource<RoutedComponentDataSource>(endpoints);
        foreach (var assembly in assemblies)
        {
            dataSource.Include(assembly);
        }

        return dataSource.ConventionBuilder;
    }

    /// <summary>
    /// Map all reactive components found in <c>Assembly.EntryAssembly()</c>.
    /// </summary>
    /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/> on which to add the components.</param>
    /// <exception cref="InvalidOperationException">When no entry assembly could be determined.</exception>
    public static IEndpointConventionBuilder MapReactiveComponents(this IEndpointRouteBuilder endpoints)
    {
        var entryAssembly = Assembly.GetEntryAssembly()
                            ?? throw new InvalidOperationException("Could not determine entry assembly.");

        return endpoints.MapReactiveComponents(entryAssembly);
    }

    /// <summary>
    /// Map all reactive components found in the given assemblies.
    /// </summary>
    /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/> on which to add the components.</param>
    /// <param name="assemblies">The assemblies in which to look for types with a <see cref="RoutedComponentAttribute"/>.</param>
    public static IEndpointConventionBuilder MapReactiveComponents(this IEndpointRouteBuilder endpoints, params Assembly[] assemblies)
    {
        var dataSource = GetOrCreateDataSource<ReactiveComponentDataSource>(endpoints);
        foreach (var assembly in assemblies)
        {
            dataSource.Include(assembly);
        }

        return dataSource.ConventionBuilder;
    }

    private static T GetOrCreateDataSource<T>(this IEndpointRouteBuilder endpoints) where T : EndpointDataSource, new()
    {
        var dataSource = endpoints.DataSources.OfType<T>().FirstOrDefault();
        if (dataSource is null)
        {
            dataSource = new T();
            endpoints.DataSources.Add(dataSource);
        }

        return dataSource;
    }
}

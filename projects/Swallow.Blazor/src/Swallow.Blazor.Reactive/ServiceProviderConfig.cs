using Microsoft.Extensions.DependencyInjection;
using Swallow.Blazor.Reactive.Abstractions.State;
using Swallow.Blazor.Reactive.Rendering;
using Swallow.Blazor.Reactive.State;

namespace Swallow.Blazor.Reactive;

/// <summary>
/// Extensions on an <see cref="IServiceCollection"/> to support reactive components.
/// </summary>
public static class ServiceProviderConfig
{
    /// <summary>
    /// Add required services for reactive rendering to the given service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which to add the registrations.</param>
    /// <returns>The given service collection with additional registrations.</returns>
    public static IServiceCollection AddReactiveRendering(this IServiceCollection services)
    {
        return services.AddScoped<StatefulReactiveComponentRenderer>()
            .AddScoped<StatefulReactiveComponentInvoker>()
            .AddScoped<IReactiveStateHandler, ReactiveStateHandler>();
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Swallow.Flux.Default;

namespace Swallow.Flux;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFlux(this IServiceCollection services)
    {
        services.TryAddScoped<IBinder, DefaultBinder>();
        services.TryAddScoped<IDispatcher, DefaultDispatcher>();
        services.TryAddScoped<IEmitter, DefaultEmitter>();

        return services;
    }

    public static IServiceCollection AddStore<TStore>(this IServiceCollection services) where TStore : class, IStore
    {
        services.AddScoped<IStore>(static sp => sp.GetRequiredService<TStore>());
        services.TryAddScoped<TStore>();

        return services;
    }

    public static IServiceCollection AddStore<TStore, TImplementation>(this IServiceCollection services)
        where TStore : class, IStore
        where TImplementation : class, TStore
    {
        services.AddScoped<IStore>(static sp => sp.GetRequiredService<TStore>());
        services.TryAddScoped<TStore, TImplementation>();

        return services;
    }
}

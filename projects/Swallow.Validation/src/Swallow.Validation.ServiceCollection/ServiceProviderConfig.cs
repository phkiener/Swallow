namespace Swallow.Validation.ServiceCollection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions to an <see cref="IServiceCollection" /> allowing a <see cref="ValidationContainer" /> to be registered and configured automatically.
/// </summary>
public static class ServiceProviderConfig
{
    /// <summary>
    ///     Register a validation container and all concrete <see cref="IAsserter{TValue}" /> found in the given assemblies.
    /// </summary>
    /// <param name="serviceCollection">The service collection to register the objects to.</param>
    /// <param name="assemblies">The assemblies to scan for asserters.</param>
    /// <returns>The service collection.</returns>
    /// <remarks>
    ///     The validation container will be populated with all found concrete asserters.
    /// </remarks>
    public static IServiceCollection AddValidationContainer(this IServiceCollection serviceCollection, params Assembly[] assemblies)
    {
        var asserterTypes = assemblies.SelectMany(a => a.GetTypes().Where(IsConcreteAsserter)).ToList();
        foreach (var type in asserterTypes)
        {
            serviceCollection.AddSingleton(type);
        }

        return serviceCollection.AddSingleton(sp => CreateValidationContainer(asserterTypes: asserterTypes, serviceProvider: sp));
    }

    private static ValidationContainer CreateValidationContainer(IEnumerable<Type> asserterTypes, IServiceProvider serviceProvider)
    {
        var container = new ValidationContainer();
        foreach (var type in asserterTypes)
        {
            var asserter = serviceProvider.GetRequiredService(type);
            container.Register(asserter);
        }

        return container;
    }

    private static bool IsConcreteAsserter(Type type)
    {
        var isAsserter = type.GetInterfaces().Count(i => i.Name == typeof(IAsserter<>).Name) == 1;
        var isConcrete = !type.IsGenericType;

        return isAsserter && isConcrete;
    }
}

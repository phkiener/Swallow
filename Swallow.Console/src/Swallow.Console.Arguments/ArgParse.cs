using System.Diagnostics.CodeAnalysis;

namespace Swallow.Console.Arguments;

public static class ArgParse
{
    private const DynamicallyAccessedMemberTypes ReflectedParts = DynamicallyAccessedMemberTypes.PublicConstructors
                                                                  | DynamicallyAccessedMemberTypes.PublicProperties;

    public static T Parse<[DynamicallyAccessedMembers(ReflectedParts)] T>(string[] args)
    {
        var remainingArgs = ParseOptions<T>(args, out var initializers);
        _ = ParseArguments<T>(remainingArgs, out var constructorArguments);

        var instance = (T)Activator.CreateInstance(typeof(T), constructorArguments)!;
        foreach (var initializer in initializers)
        {
            initializer.Invoke(instance);
        }

        return instance;
    }

    private static string[] ParseOptions<[DynamicallyAccessedMembers(ReflectedParts)] T>(string[] args, out IReadOnlyList<Action<T>> initializers)
    {
        var properties = typeof(T).GetProperties().Where(static p => p.CanWrite).ToArray();

        var setters = new List<Action<T>>();
        for (var i = 0; i < args.Length; ++i)
        {
            if (!args[i].StartsWith("--"))
            {
                initializers = setters;
                return args[i..];
            }

            var name = args[i][2..];
            var matchingProperty = properties.FirstOrDefault(p => p.PropertyType == typeof(bool) && p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (matchingProperty is not null)
            {
                setters.Add(instance => matchingProperty.SetValue(instance, true));
            }
        }

        initializers = setters;
        return [];
    }

    private static string[] ParseArguments<[DynamicallyAccessedMembers(ReflectedParts)] T>(string[] args, out object?[] arguments)
    {
        var constructors = typeof(T).GetConstructors().OrderByDescending(static c => c.GetParameters().Length);

        foreach (var constructor in constructors)
        {
            var parameters = constructor.GetParameters();
            var candidateArguments = new object?[parameters.Length];
            var isMatch = true;

            for (var i = 0; i < parameters.Length; i++)
            {
                if (!TryConvert(args[i], parameters[i].ParameterType, out object? value))
                {
                    isMatch = false;
                    break;
                }

                candidateArguments[i] = value;
            }

            if (isMatch)
            {
                arguments = candidateArguments;
                return args[candidateArguments.Length..];
            }
        }

        throw new InvalidOperationException("No suitable constructor found.");
    }

    private static bool TryConvert(string argument, Type targetType, out object? value)
    {
        if (targetType == typeof(int))
        {
            if (int.TryParse(argument, out var result))
            {
                value = result;
                return true;
            }
        }

        if (targetType == typeof(string))
        {
            value = argument;
            return true;
        }

        value = null;
        return false;
    }
}

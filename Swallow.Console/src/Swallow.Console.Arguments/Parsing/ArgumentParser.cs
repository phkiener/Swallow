using Swallow.Console.Arguments.Binding;

namespace Swallow.Console.Arguments.Parsing;

public sealed class ArgumentParser(IReadOnlyList<IBinder> binders)
{
    public T Parse<T>(string[] args)
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

    private string[] ParseOptions<T>(string[] args, out IReadOnlyList<Action<T>> initializers)
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
            var matchingProperty = properties.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (matchingProperty is null)
            {
                initializers = setters;
                return args[i..];
            }

            if (matchingProperty.PropertyType == typeof(bool))
            {
                setters.Add(instance => matchingProperty.SetValue(instance, true));
                continue;
            }

            if (i + 1 >= args.Length)
            {
                throw new InvalidOperationException($"Missing value for option {matchingProperty.Name}");
            }

            i += 1;

            if (TryConvert(args[i], matchingProperty.PropertyType, out object? value))
            {

                setters.Add(instance => matchingProperty.SetValue(instance, value));
            }
            else
            {
                throw new InvalidOperationException($"Cannot set value for option {matchingProperty.Name}");
            }
        }

        initializers = setters;
        return [];
    }

    private string[] ParseArguments<T>(string[] args, out object?[] arguments)
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

    private bool TryConvert(string argument, Type targetType, out object? value)
    {
        foreach (var binder in binders.Where(b => b.CanBind(targetType)).ToList())
        {
            if (binder.TryBind(argument, targetType, out value))
            {
                return true;
            }
        }

        value = null;
        return false;
    }
}

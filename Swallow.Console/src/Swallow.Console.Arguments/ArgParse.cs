namespace Swallow.Console.Arguments;

public static class ArgParse
{
    public static T Parse<T>(string[] args)
    {
        var constructors = typeof(T).GetConstructors().OrderByDescending(static c => c.GetParameters().Length);

        foreach (var constructor in constructors)
        {
            var parameters = constructor.GetParameters();
            var arguments = new object?[parameters.Length];
            var isMatch = true;

            for (var i = 0; i < parameters.Length; i++)
            {
                if (!TryConvert(args[i], parameters[i].ParameterType, out object? value))
                {
                    isMatch = false;
                    break;
                }

                arguments[i] = value;
            }

            if (isMatch)
            {
                return (T)constructor.Invoke(arguments);
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

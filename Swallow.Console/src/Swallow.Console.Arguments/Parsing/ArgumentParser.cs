using Swallow.Console.Arguments.Binding;

namespace Swallow.Console.Arguments.Parsing;

public sealed class ArgumentParser(IReadOnlyList<IBinder> binders)
{
    public T Parse<T>(string[] args)
    {
        var switches = Introspection.FindSwitches(typeof(T));
        var options = Introspection.FindOptions(typeof(T));
        var arguments = Introspection.FindArguments(typeof(T));

        var initializers = new List<Action<T>>();
        var constructorArguments = new List<object?>();

        Option? currentOption = null;
        bool inArguments = false;

        foreach (var arg in args)
        {
            if (arg is "--")
            {
                inArguments = true;
                continue;
            }

            if (!inArguments)
            {
                if (currentOption is not null)
                {
                    if (!TryConvert(arg, currentOption.Type, out object? optionValue))
                    {
                        throw new InvalidOperationException($"Cannot parse {arg} as {currentOption.Type} for option {currentOption.Property.Name}");
                    }

                    var option = currentOption;
                    initializers.Add(obj => option.Property.SetValue(obj, optionValue));
                    currentOption = null;

                    continue;
                }

                if (arg.StartsWith("--"))
                {
                    var matchingSwitch = switches.FirstOrDefault(s => s.Name == arg[2..]);
                    if (matchingSwitch is not null)
                    {
                        initializers.Add(obj => matchingSwitch.Property.SetValue(obj, true));
                        continue;
                    }

                    var matchingOption = options.FirstOrDefault(o => o.Name == arg[2..]);
                    if (matchingOption is not null)
                    {
                        currentOption = matchingOption;
                        continue;
                    }
                }
            }

            inArguments = true;

            var currentArgument = arguments[constructorArguments.Count];
            if (!TryConvert(arg, currentArgument.Type, out object? argumentValue))
            {
                throw new InvalidOperationException($"Cannot parse {arg} as {currentArgument.Type} for argument {constructorArguments.Count + 1}");
            }

            constructorArguments.Add(argumentValue);
        }

        var instance = (T)Activator.CreateInstance(typeof(T), constructorArguments.ToArray())!;
        foreach (var initializer in initializers)
        {
            initializer.Invoke(instance);
        }

        return instance;
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

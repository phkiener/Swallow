using System.Reflection;
using Swallow.Blazor.Reactive.Abstractions.Rendering;

namespace Swallow.Blazor.Reactive.State;

internal sealed class RegisteredState(
    IReactiveIsland island,
    string name,
    object target,
    Func<object, object?> valueFunc,
    Action<object, object?> setValueFunc)
{
    public RegisteredState(IReactiveIsland island, string name, object target, FieldInfo field)
        : this(island, name, target, field.GetValue, field.SetValue)
    {
    }

    public RegisteredState(IReactiveIsland island, string name, object target, PropertyInfo property)
        : this(island, name, target, property.GetValue, property.SetValue)
    {
    }

    public string StateEntryName => $"__state.{island.MakeIdentifier(name)}";
    public object? Value => valueFunc(target);

    public void SetValue(object? value) => setValueFunc(target, value);

    public bool IsFor(IReactiveIsland targetIsland, string targetName)
    {
        return island.MakeIdentifier(name) == targetIsland.MakeIdentifier(targetName);
    }

}

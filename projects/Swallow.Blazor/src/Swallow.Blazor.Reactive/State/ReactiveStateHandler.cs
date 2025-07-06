using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Swallow.Blazor.Reactive.Abstractions;
using Swallow.Blazor.Reactive.Abstractions.State;

namespace Swallow.Blazor.Reactive.State;

internal sealed class ReactiveStateHandler : IReactiveStateHandler, IReactiveStateProvider
{
    private readonly List<RegisteredState> registeredState = [];
    private IReadOnlyDictionary<string, string> availableState = new Dictionary<string, string>();

    public bool Register<T>(IReactiveIsland island, string name, Expression<Func<T>> propertyExpression)
    {
        if (propertyExpression.Body is not MemberExpression { Member: var member, Expression: ConstantExpression { Value: { } target } })
        {
            throw new InvalidOperationException("Handling state is only supported for properties and fields.");
        }

        var state = member switch
        {
            PropertyInfo property => new RegisteredState(island, name, target, property),
            FieldInfo field => new RegisteredState(island, name, target, field),
            _ => throw new InvalidOperationException("Handling state is only supported for properties and fields.")
        };

        registeredState.Add(state);

        if (availableState.TryGetValue($"__state.{state.Island.Build(state.Name)}", out var value) && value is not (null or ""))
        {
            var decoded = Encoding.UTF8.GetString(Base64UrlTextEncoder.Decode(value));
            var deserialized = JsonSerializer.Deserialize<T>(decoded);
            if (deserialized is not null)
            {
                state.SetValue(deserialized);
                return true;
            }
        }

        return false;
    }

    public void Remove(IReactiveIsland island, string name)
    {
        foreach (var state in registeredState.Where(s => s.Island.Build(s.Name) == island.Build(name)).ToList())
        {
            registeredState.Remove(state);
        }
    }

    public void Initialize(IReadOnlyDictionary<string, string> stateValues)
    {
        availableState = stateValues;
    }

    public IReadOnlyDictionary<string, string> Collect()
    {
        var result = new Dictionary<string, string>(capacity: registeredState.Count);
        foreach (var state in registeredState)
        {
            var currentValue = state.CurrentValue;
            if (currentValue is null)
            {
                continue;
            }

            var name = $"__state.{state.Island.Build(state.Name)}";
            var value = JsonSerializer.Serialize(currentValue);
            var encoded = Base64UrlTextEncoder.Encode(Encoding.UTF8.GetBytes(value));

            result.Add(name, encoded);
        }

        return result;
    }

    private sealed record RegisteredState(
        IReactiveIsland Island,
        string Name,
        object Target,
        Func<object, object?> ValueFunc,
        Action<object, object?> SetValueFunc)
    {
        public RegisteredState(IReactiveIsland island, string name, object target, FieldInfo field)
            : this(island, name, target, field.GetValue, field.SetValue)
        {
        }

        public RegisteredState(IReactiveIsland island, string name, object target, PropertyInfo property)
            : this(island, name, target, property.GetValue, property.SetValue)
        {
        }

        public object? CurrentValue => ValueFunc(Target);

        public void SetValue(object? value) => SetValueFunc(Target, value);
    }
}

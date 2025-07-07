using System.Linq.Expressions;
using System.Reflection;
using Swallow.Blazor.Reactive.Abstractions.Rendering;
using Swallow.Blazor.Reactive.Abstractions.State;

namespace Swallow.Blazor.Reactive.State;

internal sealed class ReactiveStateHandler(IStateSerializer serializer) : IReactiveStateHandler, IReactiveStateProvider
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

        var valueAssigned = false;
        if (availableState.TryGetValue(state.StateEntryName, out var value))
        {
            var deserialized = serializer.Deserialize<T>(value);
            if (deserialized is not null)
            {
                state.SetValue(deserialized);
                valueAssigned = true;
            }
        }

        StateChanged?.Invoke(this, EventArgs.Empty);
        return valueAssigned;
    }

    public void Remove(IReactiveIsland island, string name)
    {
        var matchingState = registeredState.Where(s => s.IsFor(island, name)).ToList();
        foreach (var state in matchingState)
        {
            registeredState.Remove(state);
        }

        if (matchingState.Any())
        {
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <inheritdoc />
    public event EventHandler? StateChanged;

    public void Initialize(IReadOnlyDictionary<string, string> stateValues)
    {
        availableState = stateValues;
    }

    public IReadOnlyDictionary<string, string> Collect()
    {
        var result = new Dictionary<string, string>(capacity: registeredState.Count);
        foreach (var state in registeredState)
        {
            if (state.Value is null)
            {
                continue;
            }

            var value = serializer.Serialize(state.Value);
            if (value is not (null or ""))
            {
                result.Add(state.StateEntryName, value);
            }
        }

        return result;
    }
}

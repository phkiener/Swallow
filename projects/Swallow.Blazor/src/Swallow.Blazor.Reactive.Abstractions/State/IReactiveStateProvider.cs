namespace Swallow.Blazor.Reactive.Abstractions.State;

/// <summary>
/// Provide and collect all available state to use for an <see cref="IReactiveStateHandler"/>.
/// </summary>
public interface IReactiveStateProvider
{
    /// <summary>
    /// Transform all available state in a key-value dictionary.
    /// </summary>
    /// <returns></returns>
    IReadOnlyDictionary<string, string> Collect();

    /// <summary>
    /// Set the given key-value dictionary as source for all initial reading of state.
    /// </summary>
    /// <param name="stateValues"></param>
    void Initialize(IReadOnlyDictionary<string, string> stateValues);

    /// <summary>
    /// Event that is triggered every time state is registered or removed.
    /// </summary>
    event EventHandler? StateChanged;
}

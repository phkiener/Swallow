namespace Swallow.Blazor.Reactive.Abstractions.State;

public interface IReactiveStateProvider
{
    IReadOnlyDictionary<string, string> Collect();

    void Initialize(IReadOnlyDictionary<string, string> stateValues);
}

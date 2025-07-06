using Swallow.Blazor.Reactive.Abstractions;

namespace Swallow.Blazor.Reactive.Rendering;

internal sealed class ReactiveIsland(string name) : IReactiveIsland
{
    public string Name { get; } = name;

    public string Build(string identifier)
    {
        return $"{Name}.{identifier}";
    }
}

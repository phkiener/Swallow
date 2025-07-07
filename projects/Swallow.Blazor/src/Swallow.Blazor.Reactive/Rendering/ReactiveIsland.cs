using Swallow.Blazor.Reactive.Abstractions.Rendering;

namespace Swallow.Blazor.Reactive.Rendering;

internal sealed class ReactiveIsland(string name) : IReactiveIsland
{
    public string Name { get; } = name;

    public string MakeIdentifier(string identifier)
    {
        return $"{Name}.{identifier}";
    }
}

internal sealed class NestedIsland(string name, IReactiveIsland parent) : IReactiveIsland
{
    public string Name { get; } = name;

    public string MakeIdentifier(string identifier)
    {
        return parent.MakeIdentifier($"{Name}.{identifier}");
    }
}

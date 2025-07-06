namespace Swallow.Blazor.Reactive.Abstractions;

/// <summary>
/// An <em>island</em> of interactivity.
/// </summary>
public interface IReactiveIsland
{
    /// <summary>
    /// Unique identifier for this island.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Build a unique identifier by combining the island's <see cref="Name"/> with the given
    /// identifier.
    /// </summary>
    /// <param name="identifier">The identifier of an element.</param>
    /// <returns>A fully built identifier.</returns>
    public string Build(string identifier);
}

using Microsoft.AspNetCore.Components;
using Swallow.Blazor.Reactive.Abstractions.Rendering;
using Swallow.Blazor.Reactive.Rendering;

namespace Swallow.Blazor.Reactive.Components;

/// <summary>
/// A sub-section inside a <see cref="ReactiveBoundary"/> that will prefix all generated identifiers
/// with a given <see cref="ReactiveSection.Name"/>.
/// </summary>
public partial class ReactiveSection : ComponentBase
{
    private IReactiveIsland? innerIsland;

    [CascadingParameter]
    private IReactiveIsland? ReactiveIsland { get; set; }

    /// <summary>
    /// The name for this section inside the parent <see cref="IReactiveIsland"/>.
    /// </summary>
    [Parameter, EditorRequired]
    public required string Name { get; set; }

    /// <summary>
    /// The content for the inner <see cref="IReactiveIsland"/>.
    /// </summary>
    [Parameter, EditorRequired]
    public required RenderFragment<IReactiveIsland> ChildContent { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (ReactiveIsland is null)
        {
            throw new InvalidOperationException($"{nameof(ReactiveSection)} can only be used inside a {nameof(ReactiveBoundary)}.");
        }

        innerIsland = new NestedIsland(Name, ReactiveIsland);
    }
}


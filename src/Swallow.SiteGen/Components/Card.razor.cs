using Microsoft.AspNetCore.Components;

namespace Swallow.SiteGen.Components;

public sealed partial class Card : ComponentBase
{
    [Parameter]
    [EditorRequired]
    public required string Title { get; set; }

    [Parameter]
    [EditorRequired]
    public required RenderFragment Content { get; set; }

    [Parameter]
    public RenderFragment? Footer { get; set; }
}

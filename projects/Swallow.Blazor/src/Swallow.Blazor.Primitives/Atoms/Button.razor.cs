using Microsoft.AspNetCore.Components;

namespace Swallow.Blazor.Primitives.Atoms;

public partial class Button : ComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get => Content; set => Content = value; }

    [Parameter]
    public RenderFragment? Leading { get; set; }

    [Parameter]
    public RenderFragment? Content { get; set; }

    [Parameter]
    public RenderFragment? Trailing { get; set; }
}

using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Swallow.SiteGen.Components;

public enum Brand { GitHub, NuGet }

public sealed partial class BrandLink : ComponentBase
{
    [Parameter]
    [EditorRequired]
    public required Brand Brand { get; set; }

    [Parameter]
    [EditorRequired]
    public required string Href { get; set; }

    [Parameter]
    [EditorRequired]
    public required RenderFragment ChildContent { get; set; }

    private string BrandClass => Brand switch
    {
        Brand.GitHub => "github",
        Brand.NuGet => "nuget",
        _ => throw new InvalidEnumArgumentException(nameof(Brand), (int)Brand, typeof(Brand))
    };
}

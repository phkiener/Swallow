using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Endpoints;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Http;

namespace Swallow.Blazor.Reactive.DataSource;

internal sealed class ReactiveComponentRoot : ComponentBase
{
    private Type? renderedComponentType;

    [CascadingParameter]
    public required HttpContext HttpContext { get; set; }

    protected override void OnInitialized()
    {
        renderedComponentType = HttpContext.GetEndpoint()?.Metadata.GetRequiredMetadata<ComponentTypeMetadata>().Type;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (renderedComponentType is not null)
        {
            builder.OpenComponent(0, renderedComponentType);
            builder.CloseComponent();
        }
    }
}

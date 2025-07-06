using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Http;
using Swallow.Blazor.Reactive.Abstractions;
using Swallow.Blazor.Reactive.Abstractions.State;

namespace Swallow.Blazor.Reactive.DataSource;

internal sealed class StatefulReactiveComponentRoot(IReactiveStateHandler stateHandler) : ComponentBase
{
    private Type? renderedComponentType;

    [Parameter, EditorRequired]
    public required HttpContext HttpContext { get; set; }

    [Parameter, EditorRequired]
    public required IReactiveIsland Island { get; set; }

    protected override void OnInitialized()
    {
        renderedComponentType = HttpContext.GetEndpoint()?.Metadata.GetRequiredMetadata<StatefulReactiveComponentMetadata>().ComponentType;
    }

    protected override void BuildRenderTree(RenderTreeBuilder b)
    {
        if (renderedComponentType is not null)
        {
            b.OpenComponent<CascadingValue<IReactiveIsland>>(0);
            b.AddComponentParameter(1, nameof(CascadingValue<IReactiveIsland>.IsFixed), true);
            b.AddComponentParameter(2, nameof(CascadingValue<IReactiveIsland>.Value), Island);
            b.AddComponentParameter(3, nameof(CascadingValue<IReactiveIsland>.ChildContent), new RenderFragment(bb =>
            {
                bb.OpenComponent<CascadingValue<IReactiveStateHandler>>(0);
                bb.AddComponentParameter(1, nameof(CascadingValue<IReactiveStateHandler>.IsFixed), true);
                bb.AddComponentParameter(2, nameof(CascadingValue<IReactiveStateHandler>.Value), stateHandler);
                bb.AddComponentParameter(3, nameof(CascadingValue<IReactiveStateHandler>.ChildContent), new RenderFragment(bbb =>
                {
                    bbb.OpenComponent<CascadingValue<HttpContext>>(0);
                    bbb.AddComponentParameter(1, nameof(CascadingValue<HttpContext>.IsFixed), true);
                    bbb.AddComponentParameter(2, nameof(CascadingValue<HttpContext>.Value), HttpContext);
                    bbb.AddComponentParameter(3, nameof(CascadingValue<HttpContext>.ChildContent), new RenderFragment(bbbb =>
                    {
                        bbbb.OpenComponent(0, renderedComponentType);
                        bbbb.CloseComponent();
                    }));
                    bbb.CloseComponent();
                }));
                bb.CloseComponent();
            }));
            b.CloseComponent();
        }
    }
}

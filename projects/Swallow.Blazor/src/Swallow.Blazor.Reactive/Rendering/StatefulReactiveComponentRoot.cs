using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Http;
using Swallow.Blazor.Reactive.Abstractions.Rendering;
using Swallow.Blazor.Reactive.Abstractions.State;

namespace Swallow.Blazor.Reactive.Rendering;

internal sealed class StatefulReactiveComponentRoot(IReactiveStateHandler stateHandler) : ComponentBase
{
    [Parameter, EditorRequired]
    public required HttpContext HttpContext { get; set; }

    [Parameter, EditorRequired]
    public required Type ComponentType { get; set; }

    [Parameter, EditorRequired]
    public required IReactiveIsland Island { get; set; }

    private readonly Dictionary<string, object> componentParameters = new();

    protected override void OnInitialized()
    {
        foreach (var parameter in ComponentType.GetProperties().Where(static p => p.GetCustomAttribute<ParameterAttribute>() is not null))
        {
            if (HttpContext.Request.Query.TryGetValue(parameter.Name, out var value))
            {
                try
                {
                    var converted = Convert.ChangeType(value.ToString(), parameter.PropertyType, CultureInfo.InvariantCulture);
                    componentParameters[parameter.Name] = converted;
                }
                catch (Exception e) when (e is InvalidCastException or FormatException)
                {
                }
            }
        }
    }

    // Written by hand so that the component itself can be internal
    protected override void BuildRenderTree(RenderTreeBuilder b)
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
                    bbbb.OpenComponent(0, ComponentType);

                    foreach (var parameter in componentParameters)
                    {
                        bbbb.AddComponentParameter(1, parameter.Key, parameter.Value);
                    }

                    bbbb.CloseComponent();
                }));
                bbb.CloseComponent();
            }));
            bb.CloseComponent();
        }));
        b.CloseComponent();
    }
}

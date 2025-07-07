using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Endpoints;

namespace Swallow.Blazor.Reactive.Routing;

internal static class EndpointBuilderExtensions
{
    private static object? NoRenderModeMetadata { get; } = BuildRenderModeMetadata();

    private static object? BuildRenderModeMetadata()
    {
        var definingAssembly = typeof(IRazorComponentEndpointInvoker).Assembly;
        var metadataType = definingAssembly.GetType("Microsoft.AspNetCore.Components.Endpoints.ConfiguredRenderModesMetadata");
        if (metadataType is null)
        {
            return null;
        }

        return Activator.CreateInstance(metadataType, [Array.Empty<IComponentRenderMode>()]);
    }

    public static void AddEmptyRenderMode(this EndpointBuilder endpointBuilder)
    {
        // If the metadata is null, something has changed in the framework and we haven't noticed. That's okay.
        // The endpoint renderer will write (empty) state into a comment in the DOM... useless and noisy but not an issue.
        if (NoRenderModeMetadata is not null)
        {
            endpointBuilder.Metadata.Add(NoRenderModeMetadata);
        }
    }

    public static void CopyAttributeMetadata(this EndpointBuilder endpointBuilder, Type sourceType, Func<Attribute, bool> excludeAttribute)
    {
        foreach (var attribute in sourceType.GetCustomAttributes<Attribute>(inherit: true))
        {
            if (attribute is not RequiredMemberAttribute && !excludeAttribute(attribute))
            {
                endpointBuilder.Metadata.Add(attribute);
            }
        }
    }
}

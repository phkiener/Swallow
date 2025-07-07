using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Endpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.DependencyInjection;
using Swallow.Blazor.Reactive.Abstractions;

namespace Swallow.Blazor.Reactive.Routing;

internal sealed class RoutedComponentDataSource : AttributeBasedDataSource<RoutedComponentAttribute>
{
    protected override IReadOnlyList<EndpointBuilder> BuildEndpoints(Type targetType)
    {
        var builders = new List<EndpointBuilder>();
        foreach (var attribute in targetType.GetCustomAttributes<RoutedComponentAttribute>())
        {
            var endpointBuilder = new RouteEndpointBuilder(
                requestDelegate: RenderComponent,
                routePattern: RoutePatternFactory.Parse(attribute.RouteTemplate),
                order: 0);

            endpointBuilder.DisplayName = $"{endpointBuilder.RoutePattern.RawText} ({targetType.Name})";
            endpointBuilder.Metadata.Add(attribute);
            endpointBuilder.Metadata.Add(new HttpMethodMetadata(attribute.Methods));
            endpointBuilder.Metadata.Add(new ComponentTypeMetadata(targetType));
            endpointBuilder.Metadata.Add(new RootComponentMetadata(targetType));

            endpointBuilder.AddEmptyRenderMode();
            endpointBuilder.CopyAttributeMetadata(targetType, static a => a is RoutedComponentAttribute);

            builders.Add(endpointBuilder);
        }

        return builders;
    }

    private static Task RenderComponent(HttpContext httpContext)
    {
        var invoker = httpContext.RequestServices.GetRequiredService<IRazorComponentEndpointInvoker>();
        return invoker.Render(httpContext);
    }
}

using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Endpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.DependencyInjection;
using Swallow.Blazor.Reactive.Abstractions;
using Swallow.Blazor.Reactive.Rendering;

namespace Swallow.Blazor.Reactive.Routing;

internal sealed class ReactiveComponentDataSource : AttributeBasedDataSource<ReactiveComponentAttribute>
{
    protected override IReadOnlyList<EndpointBuilder> BuildEndpoints(Type targetType)
    {
        var attribute = targetType.GetCustomAttribute<ReactiveComponentAttribute>()!;
        var routeTemplate = attribute.RouteTemplate ?? $"_rx/{targetType.Assembly.GetName().Name}/{targetType.FullName}";

        var endpointBuilder = new RouteEndpointBuilder(
            requestDelegate: RenderReactiveComponent,
            routePattern: RoutePatternFactory.Parse(routeTemplate),
            order: 0);

        endpointBuilder.DisplayName = $"{endpointBuilder.RoutePattern.RawText} ({targetType.Name})";
        endpointBuilder.Metadata.Add(routeTemplate);
        endpointBuilder.Metadata.Add(new HttpMethodMetadata([HttpMethods.Post]));
        endpointBuilder.Metadata.Add(new ComponentTypeMetadata(targetType));
        endpointBuilder.AddEmptyRenderMode();
        endpointBuilder.CopyAttributeMetadata(targetType, static a => a is ReactiveComponentAttribute);

        return [endpointBuilder];
    }

    private static Task RenderReactiveComponent(HttpContext httpContext)
    {
        var islandIdentifier = httpContext.Request.Headers["rx-island"].ToString();
        if (islandIdentifier is null or "")
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            return httpContext.Response.CompleteAsync();
        }

        var componentType = httpContext.GetEndpoint()?.Metadata.GetMetadata<ComponentTypeMetadata>()?.Type;
        if (componentType is null)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            return httpContext.Response.CompleteAsync();
        }

        var invoker = httpContext.RequestServices.GetRequiredService<StatefulReactiveComponentInvoker>();
        return invoker.Render(httpContext, islandIdentifier, componentType);
    }
}

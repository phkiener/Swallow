using System.Diagnostics.CodeAnalysis;

namespace Swallow.Blazor.Reactive.Abstractions;

/// <summary>
/// Marker attribute for a component that may be rendered inside a reactive boundary.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class StatefulReactiveComponentAttribute : Attribute
{
    /// <summary>
    /// Route template to which the component will be mapped to.
    /// </summary>
    public string? RouteTemplate { get; }

    /// <summary>
    /// Create an instance of <see cref="StatefulReactiveComponentAttribute"/> using the default route.
    /// </summary>
    public StatefulReactiveComponentAttribute() : this(null)
    {
    }

    /// <summary>
    /// Create an instance of <see cref="StatefulReactiveComponentAttribute"/>.
    /// </summary>
    /// <param name="routeTemplate">Route template to which the component will be mapped to.</param>
    public StatefulReactiveComponentAttribute([StringSyntax("Route")] string? routeTemplate)
    {
        RouteTemplate = routeTemplate;
    }
}

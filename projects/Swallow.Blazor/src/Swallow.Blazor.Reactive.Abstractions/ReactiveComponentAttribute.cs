using System.Diagnostics.CodeAnalysis;

namespace Swallow.Blazor.Reactive.Abstractions;

/// <summary>
/// Marker attribute for a component that may be rendered inside a reactive boundary.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class ReactiveComponentAttribute : Attribute
{
    /// <summary>
    /// Route template to which the component will be mapped to.
    /// </summary>
    public string? RouteTemplate { get; }

    /// <summary>
    /// Create an instance of <see cref="ReactiveComponentAttribute"/> using the default route.
    /// </summary>
    public ReactiveComponentAttribute() : this(null)
    {
    }

    /// <summary>
    /// Create an instance of <see cref="ReactiveComponentAttribute"/>.
    /// </summary>
    /// <param name="routeTemplate">Route template to which the component will be mapped to.</param>
    public ReactiveComponentAttribute([StringSyntax("Route")] string? routeTemplate)
    {
        RouteTemplate = routeTemplate;
    }
}

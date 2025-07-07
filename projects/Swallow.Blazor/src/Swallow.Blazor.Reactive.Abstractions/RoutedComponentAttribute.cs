using System.Diagnostics.CodeAnalysis;

namespace Swallow.Blazor.Reactive.Abstractions;

/// <summary>
/// Marker attribute for a component that supports direct routing.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class RoutedComponentAttribute : Attribute
{
    /// <summary>
    /// Route template to which the component will be mapped to.
    /// </summary>
    public string RouteTemplate { get; }

    /// <summary>
    /// All methods for which the component will be mapped to.
    /// </summary>
    public string[] Methods { get; }

    /// <summary>
    /// Create an instance of <see cref="RoutedComponentAttribute"/>.
    /// </summary>
    /// <param name="routeTemplate">Route template to which the component will be mapped to.</param>
    /// <param name="methods">All methods for which the component will be mapped to.</param>
    /// <remarks>Not passing any <paramref name="methods"/> means that <em>all</em> methods are allowed.</remarks>
    public RoutedComponentAttribute([StringSyntax("Route")] string routeTemplate, params string[] methods)
    {
        RouteTemplate = routeTemplate;
        Methods = methods;
    }
}

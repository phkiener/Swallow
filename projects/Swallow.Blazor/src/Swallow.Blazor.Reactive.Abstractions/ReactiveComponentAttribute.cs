using System.Diagnostics.CodeAnalysis;

namespace Swallow.Blazor.Reactive.Abstractions;

/// <summary>
/// Marker attribute for a component that supports reactive rendering.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class ReactiveComponentAttribute : Attribute
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
    /// Create an instance of <see cref="ReactiveComponentAttribute"/>.
    /// </summary>
    /// <param name="routeTemplate">Route template to which the component will be mapped to.</param>
    /// <param name="methods">All methods for which the component will be mapped to.</param>
    /// <remarks>Not passing any <paramref name="methods"/> means that <em>all</em> methods are allowed.</remarks>
    public ReactiveComponentAttribute([StringSyntax("Route")] string routeTemplate, params IEnumerable<string> methods)
    {
        RouteTemplate = routeTemplate;
        Methods = methods.ToArray();
    }
}

/// <summary>
/// Marker attribute for a component that supports reactive rendering via GET requests.
/// </summary>
public sealed class ReactiveComponentGetAttribute : ReactiveComponentAttribute
{
    /// <summary>
    /// Create an instance of <see cref="ReactiveComponentAttribute"/> that supports only GET requests.
    /// </summary>
    /// <param name="routeTemplate">Route template to which the component will be mapped to.</param>
    public ReactiveComponentGetAttribute([StringSyntax("Route")] string routeTemplate)
        : base(routeTemplate, HttpMethod.Get.Method)
    {
    }
}

/// <summary>
/// Marker attribute for a component that supports reactive rendering via POST requests.
/// </summary>
public sealed class ReactiveComponentPostAttribute : ReactiveComponentAttribute
{
    /// <summary>
    /// Create an instance of <see cref="ReactiveComponentAttribute"/> that supports only POST requests.
    /// </summary>
    /// <param name="routeTemplate">Route template to which the component will be mapped to.</param>
    public ReactiveComponentPostAttribute([StringSyntax("Route")] string routeTemplate)
        : base(routeTemplate, HttpMethod.Post.Method)
    {
    }
}

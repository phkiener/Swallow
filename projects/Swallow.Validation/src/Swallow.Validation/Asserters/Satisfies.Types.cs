using Swallow.Validation.Next.Asserters.Types;

namespace Swallow.Validation.Next.Asserters;

partial class Satisfies
{
    /// <summary>
    /// Returns an <see cref="IsInstanceOfAsserter"/> that checks whether a given value is of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to check against.</typeparam>
    public static IAsserter<object> IsType<T>() => IsType(typeof(T));

    /// <summary>
    /// Returns an <see cref="IsInstanceOfAsserter"/> that checks whether a given value is assignable to <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to check against.</typeparam>
    public static IAsserter<object> IsAssignableTo<T>() => IsAssignableTo(typeof(T));

    /// <summary>
    /// Returns an <see cref="IsInstanceOfAsserter"/> that checks whether a given value is of type <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type to check against.</param>
    public static IAsserter<object> IsType(Type type) => new IsInstanceOfAsserter(type);

    /// <summary>
    /// Returns an <see cref="IsInstanceOfAsserter"/> that checks whether a given value is assignable to <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type to check against.</param>
    public static IAsserter<object> IsAssignableTo(Type type) => new IsInstanceOfAsserter(type, allowDerivedTypes: true);
}

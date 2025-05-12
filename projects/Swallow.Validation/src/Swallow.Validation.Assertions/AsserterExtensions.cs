namespace Swallow.Validation;

using Errors;
using Internal;

/// <summary>
///     Extensions to aid in using an <see cref="IAsserter{TValue}" />.
/// </summary>
public static class AsserterExtensions
{
    /// <summary>
    ///     Call <see cref="IAsserter{TValue}.Check" /> without providing a <see cref="INamedValueProvider{T}" />.
    /// </summary>
    /// <param name="asserter">The asserter to call.</param>
    /// <param name="value">The value to assert.</param>
    /// <param name="error">The resulting error - if any - or <c>null</c>.</param>
    /// <typeparam name="T">Type of the asserted value.</typeparam>
    /// <returns><c>true</c> if the validation succeeds, <c>false</c> if an error is produced.</returns>
    /// <remarks>
    ///     <p>
    ///         The value is given the name 'value' for assertion.
    ///     </p>
    /// </remarks>
    public static bool Check<T>(this IAsserter<T> asserter, T value, out ValidationError error)
    {
        return asserter.Check(value, "value", out error);
    }

    /// <summary>
    ///     Call <see cref="IAsserter{TValue}.Check" /> without providing a <see cref="INamedValueProvider{T}" />.
    /// </summary>
    /// <param name="asserter">The asserter to call.</param>
    /// <param name="value">The value to assert.</param>
    /// <param name="name">The name to give the asserted value.</param>
    /// <param name="error">The resulting error - if any - or <c>null</c>.</param>
    /// <typeparam name="T">Type of the asserted value.</typeparam>
    /// <returns><c>true</c> if the validation succeeds, <c>false</c> if an error is produced.</returns>
    public static bool Check<T>(this IAsserter<T> asserter, T value, string name, out ValidationError error)
    {
        return asserter.Check(new NamedStoredValue<T>(value, name), out error);
    }
}

namespace Swallow.Validation.Assertions;

using System;
using Errors;

/// <summary>
///     Extensions for checking type properties of an object.
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    ///     Check that the asserted object is of the given type.
    /// </summary>
    /// <param name="assertable">The asserted value.</param>
    /// <param name="type">The type that the value should be.</param>
    /// <returns>An assertion containing the check.</returns>
    public static IAssertion<object> IsType(this IAssertable<object> assertable, Type type)
    {
        return assertable.Satisfies(
            predicate: o => o.GetType() == type,
            errorFunc: o => new TypeValidationError(expectedType: type, actualType: o.GetType(), mode: TypeValidationError.MatchingMode.SameType));
    }

    /// <summary>
    ///     Check that the asserted object can be assigned to the given type.
    /// </summary>
    /// <param name="assertable">The asserted value.</param>
    /// <param name="type">The type that the value should be assignable to.</param>
    /// <returns>An assertion containing the check.</returns>
    public static IAssertion<object> IsAssignableTo(this IAssertable<object> assertable, Type type)
    {
        return assertable.Satisfies(
            predicate: type.IsInstanceOfType,
            errorFunc: o => new TypeValidationError(
                expectedType: type,
                actualType: o.GetType(),
                mode: TypeValidationError.MatchingMode.AssignableTo));
    }

    /// <summary>
    ///     Check that the asserted object can be assigned to the given type.
    /// </summary>
    /// <param name="assertable">The asserted value.</param>
    /// <typeparam name="T">The type that the value should be.</typeparam>
    /// <returns>An assertion containing the check.</returns>
    public static IAssertion<object> IsType<T>(this IAssertable<object> assertable)
    {
        return assertable.IsType(typeof(T));
    }

    /// <summary>
    ///     Check that the asserted object is of the given type.
    /// </summary>
    /// <param name="assertable">The asserted value.</param>
    /// <typeparam name="T">The type that the value should be assignable to.</typeparam>
    /// <returns>An assertion containing the check.</returns>
    public static IAssertion<object> IsAssignableTo<T>(this IAssertable<object> assertable)
    {
        return assertable.IsAssignableTo(typeof(T));
    }
}

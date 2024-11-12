namespace Swallow.Validation.Assertions;

using System;
using System.Collections.Generic;
using System.Linq;
using Errors;
using Internal;

/// <summary>
///     Extensions for validating collections in a shorter, prettier way.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    ///     Checks that a collection is not empty.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <typeparam name="T">Type of the value in the collection to check.</typeparam>
    /// <returns>The constructed assertion.</returns>
    public static IAssertion<IEnumerable<T>> IsNotEmpty<T>(this IAssertable<IEnumerable<T>> assertion)
    {
        return assertion.Satisfies(predicate: x => x.Any(), errorFunc: _ => new EmptyCollectionValidationError());
    }

    /// <summary>
    ///     Checks that a value is contained in a certain set of values.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="allowedValues">The collection of allowed values.</param>
    /// <typeparam name="T">The type of the value to check.</typeparam>
    /// <returns>The constructed assertion.</returns>
    public static IAssertion<T> IsIn<T>(this IAssertable<T> assertion, ICollection<T> allowedValues)
    {
        return assertion.Satisfies(predicate: allowedValues.Contains, errorFunc: _ => DisallowedValueValidationError<T>.BeOneOf(allowedValues));
    }

    /// <summary>
    ///     Checks that a value is not contained in a certain set of values.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="disallowedValues">The collection of disallowed values.</param>
    /// <typeparam name="T">The type of the value to check.</typeparam>
    /// <returns>The constructed assertion.</returns>
    public static IAssertion<T> IsNotIn<T>(this IAssertable<T> assertion, ICollection<T> disallowedValues)
    {
        return assertion.Satisfies(
            predicate: value => !disallowedValues.Contains(value),
            errorFunc: _ => DisallowedValueValidationError<T>.NotBeOneOf(disallowedValues));
    }

    /// <summary>
    ///     Checks that all elements of a collection satisfy the given validation.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="elementAssertion">The assertion for each single element.</param>
    /// <typeparam name="T">Type of the elements in the collection.</typeparam>
    /// <returns>The constructed assertion.</returns>
    /// <remarks>
    ///     The assertion for elements can be any assertion you'd use in the context of a validator (in other words; everything after a
    ///     `That()`).
    /// </remarks>
    public static IAssertion<IEnumerable<T>> HasAll<T>(
        this IAssertable<IEnumerable<T>> assertion,
        Func<IUnformattedAssertable<T>, IValidation> elementAssertion)
    {
        return assertion.Satisfies(new CollectionAsserter<T>(elementAssertion));
    }

    /// <summary>
    ///     Checks that all elements of a collection are valid using their own validation method.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <typeparam name="T">Type of the elements in the collection.</typeparam>
    /// <returns>The constructed assertion.</returns>
    public static IAssertion<IEnumerable<T>> HasAllValid<T>(this IAssertable<IEnumerable<T>> assertion) where T : IValidatable
    {
        return assertion.HasAll(x => x.IsValid());
    }
}

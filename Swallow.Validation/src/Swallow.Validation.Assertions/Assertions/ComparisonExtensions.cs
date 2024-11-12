namespace Swallow.Validation.Assertions;

using System;
using Errors;

/// <summary>
///     Extensions providing comparison-validations in a prettier way.
/// </summary>
public static class ComparisonExtensions
{
    /// <summary>
    ///     Checks that a value is inside a certain range.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="lowerInclusive">The lower bound, inclusive.</param>
    /// <param name="upperInclusive">The upper bound, inclusive.</param>
    /// <typeparam name="T">Type of the value that is asserted.</typeparam>
    /// <returns>The constructed assertion.</returns>
    public static IAssertion<T> IsInRange<T>(this IAssertable<T> assertion, T lowerInclusive, T upperInclusive) where T : IComparable<T>
    {
        return assertion.Satisfies(
            predicate: x => x.CompareTo(lowerInclusive) >= 0 && x.CompareTo(upperInclusive) <= 0,
            errorFunc: _ => new RangeValidationError<T>(lowerBound: lowerInclusive, upperBound: upperInclusive));
    }

    /// <summary>
    ///     Checks that a value is inside a certain range.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="lowerInclusive">The lower bound, inclusive.</param>
    /// <param name="upperInclusive">The upper bound, inclusive.</param>
    /// <typeparam name="T">Type of the value that is asserted.</typeparam>
    /// <returns>The constructed assertion.</returns>
    /// <remarks>Always fails when the asserted value is <c>null</c>.</remarks>
    public static IAssertion<T?> IsInRange<T>(this IAssertable<T?> assertion, T lowerInclusive, T upperInclusive) where T : struct, IComparable<T>
    {
        return assertion.Satisfies(
            predicate: x => x is not null && x.Value.CompareTo(lowerInclusive) >= 0 && x.Value.CompareTo(upperInclusive) <= 0,
            errorFunc: _ => new RangeValidationError<T>(lowerBound: lowerInclusive, upperBound: upperInclusive));
    }

    /// <summary>
    ///     Checks that a certain object is not null.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <typeparam name="T">Type of the class to check.</typeparam>
    /// <returns>The constructed assertion.</returns>
    public static IAssertion<T> IsNotNull<T>(this IAssertable<T> assertion) where T : class
    {
        return assertion.Satisfies(predicate: x => x != null, errorFunc: _ => new IsNullValidationError());
    }

    /// <summary>
    ///     Checks that a certain nullable value is not null.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <typeparam name="T">Type of the value to check.</typeparam>
    /// <returns>The constructed assertion.</returns>
    public static IAssertion<T?> IsNotNull<T>(this IAssertable<T?> assertion) where T : struct
    {
        return assertion.Satisfies(predicate: x => x != null, errorFunc: _ => new IsNullValidationError());
    }

    /// <summary>
    ///     Checks that a value is greater than a certain threshold.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="lowerBound">The lower bound to compare against.</param>
    /// <typeparam name="T">The type of the assertion.</typeparam>
    /// <returns>The constructed assertion.</returns>
    public static IAssertion<T> IsGreaterThan<T>(this IAssertable<T> assertion, T lowerBound) where T : IComparable<T>
    {
        return assertion.Satisfies(
            predicate: x => x.CompareTo(lowerBound) > 0,
            errorFunc: _ => RangeValidationError<T>.FromLowerBound(value: lowerBound, isInclusive: false));
    }

    /// <summary>
    ///     Checks that a value is greater than a certain threshold.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="lowerBound">The lower bound to compare against.</param>
    /// <typeparam name="T">The type of the assertion.</typeparam>
    /// <returns>The constructed assertion.</returns>
    /// <remarks>Always fails when the asserted value is <c>null</c>.</remarks>
    public static IAssertion<T?> IsGreaterThan<T>(this IAssertable<T?> assertion, T lowerBound) where T : struct, IComparable<T>
    {
        return assertion.Satisfies(
            predicate: x => x is not null && x.Value.CompareTo(lowerBound) > 0,
            errorFunc: _ => RangeValidationError<T>.FromLowerBound(value: lowerBound, isInclusive: false));
    }

    /// <summary>
    ///     Checks that a value is greater than or equal to a certain threshold.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="lowerBound">The lower bound to compare against.</param>
    /// <typeparam name="T">The type of the assertion.</typeparam>
    /// <returns>The constructed assertion.</returns>
    public static IAssertion<T> IsGreaterThanOrEqual<T>(this IAssertable<T> assertion, T lowerBound) where T : IComparable<T>
    {
        return assertion.Satisfies(
            predicate: x => x.CompareTo(lowerBound) >= 0,
            errorFunc: _ => RangeValidationError<T>.FromLowerBound(value: lowerBound, isInclusive: true));
    }

    /// <summary>
    ///     Checks that a value is greater than or equal to a certain threshold.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="lowerBound">The lower bound to compare against.</param>
    /// <typeparam name="T">The type of the assertion.</typeparam>
    /// <returns>The constructed assertion.</returns>
    /// <remarks>Always fails when the asserted value is <c>null</c>.</remarks>
    public static IAssertion<T?> IsGreaterThanOrEqual<T>(this IAssertable<T?> assertion, T lowerBound) where T : struct, IComparable<T>
    {
        return assertion.Satisfies(
            predicate: x => x is not null && x.Value.CompareTo(lowerBound) >= 0,
            errorFunc: _ => RangeValidationError<T>.FromLowerBound(value: lowerBound, isInclusive: true));
    }

    /// <summary>
    ///     Checks that a value is less than a certain threshold.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="upperBound">The upper bound to compare against.</param>
    /// <typeparam name="T">The type of the assertion.</typeparam>
    /// <returns>The constructed assertion.</returns>
    public static IAssertion<T> IsLessThan<T>(this IAssertable<T> assertion, T upperBound) where T : IComparable<T>
    {
        return assertion.Satisfies(
            predicate: x => x.CompareTo(upperBound) < 0,
            errorFunc: _ => RangeValidationError<T>.FromUpperBound(value: upperBound, isInclusive: false));
    }

    /// <summary>
    ///     Checks that a value is less than a certain threshold.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="upperBound">The upper bound to compare against.</param>
    /// <typeparam name="T">The type of the assertion.</typeparam>
    /// <returns>The constructed assertion.</returns>
    public static IAssertion<T?> IsLessThan<T>(this IAssertable<T?> assertion, T upperBound) where T : struct, IComparable<T>
    {
        return assertion.Satisfies(
            predicate: x => x is not null && x.Value.CompareTo(upperBound) < 0,
            errorFunc: _ => RangeValidationError<T>.FromUpperBound(value: upperBound, isInclusive: false));
    }

    /// <summary>
    ///     Checks that a value is less than or equal to a certain threshold.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="upperBound">The upper bound to compare against.</param>
    /// <typeparam name="T">The type of the assertion.</typeparam>
    /// <returns>The constructed assertion.</returns>
    public static IAssertion<T> IsLessThanOrEqual<T>(this IAssertable<T> assertion, T upperBound) where T : IComparable<T>
    {
        return assertion.Satisfies(
            predicate: x => x.CompareTo(upperBound) <= 0,
            errorFunc: _ => RangeValidationError<T>.FromUpperBound(value: upperBound, isInclusive: true));
    }

    /// <summary>
    ///     Checks that a value is less than or equal to a certain threshold.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="upperBound">The upper bound to compare against.</param>
    /// <typeparam name="T">The type of the assertion.</typeparam>
    /// <returns>The constructed assertion.</returns>
    /// <remarks>Always fails when the asserted value is <c>null</c>.</remarks>
    public static IAssertion<T?> IsLessThanOrEqual<T>(this IAssertable<T?> assertion, T upperBound) where T : struct, IComparable<T>
    {
        return assertion.Satisfies(
            predicate: x => x is not null && x.Value.CompareTo(upperBound) <= 0,
            errorFunc: _ => RangeValidationError<T>.FromUpperBound(value: upperBound, isInclusive: true));
    }

    /// <summary>
    ///     Checks that a value is equal to another value.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="value">The value that is required.</param>
    /// <typeparam name="T">The type of the assertion.</typeparam>
    /// <returns>The constructed assertion.</returns>
    public static IAssertion<T> IsEqualTo<T>(this IAssertable<T> assertion, T value) where T : IEquatable<T>
    {
        return assertion.Satisfies(predicate: x => x.Equals(value), errorFunc: _ => EqualityValidationError<T>.MustBe(value));
    }

    /// <summary>
    ///     Checks that a value is equal to another value.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="value">The value that is required.</param>
    /// <typeparam name="T">The type of the assertion.</typeparam>
    /// <returns>The constructed assertion.</returns>
    /// <remarks>Always fails when the asserted value is <c>null</c>.</remarks>
    public static IAssertion<T?> IsEqualTo<T>(this IAssertable<T?> assertion, T value) where T : struct, IEquatable<T>
    {
        return assertion.Satisfies(predicate: x => x.Equals(value), errorFunc: _ => EqualityValidationError<T>.MustBe(value));
    }

    /// <summary>
    ///     Checks that a value is not equal to another value.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="value">The value that is forbidden.</param>
    /// <typeparam name="T">The type of the assertion.</typeparam>
    /// <returns>The constructed assertion.</returns>
    public static IAssertion<T> IsNotEqualTo<T>(this IAssertable<T> assertion, T value) where T : IEquatable<T>
    {
        return assertion.Satisfies(predicate: x => !x.Equals(value), errorFunc: _ => EqualityValidationError<T>.MustNotBe(value));
    }

    /// <summary>
    ///     Checks that a value is not equal to another value.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="value">The value that is forbidden.</param>
    /// <typeparam name="T">The type of the assertion.</typeparam>
    /// <returns>The constructed assertion.</returns>
    /// <remarks>Always passes when the asserted value is <c>null</c>.</remarks>
    public static IAssertion<T?> IsNotEqualTo<T>(this IAssertable<T?> assertion, T value) where T : struct, IEquatable<T>
    {
        return assertion.Satisfies(predicate: x => !x.Equals(value), errorFunc: _ => EqualityValidationError<T>.MustNotBe(value));
    }
}

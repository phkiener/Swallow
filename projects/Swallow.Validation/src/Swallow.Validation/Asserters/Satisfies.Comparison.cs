using Swallow.Validation.Next.Asserters.Comparison;

namespace Swallow.Validation.Next.Asserters;

partial class Satisfies
{
    /// <summary>
    /// Returns a new <see cref="IsEqualToAsserter{T}"/> that checks whether a given value is equal to <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value to check against.</param>
    /// <typeparam name="T">Type of the value to check.</typeparam>
    public static IAsserter<T> EqualTo<T>(T value) where T : IEquatable<T> => new IsEqualToAsserter<T>(value);

    /// <summary>
    /// Returns a new <see cref="IsNotNullAsserter{T}"/> that checks whether a given object is <c>null</c>.
    /// </summary>
    public static IAsserter<object?> NotNull => new IsNotNullAsserter<object?>();

    /// <summary>
    /// Return a new instance of <see cref="IsInRangeAsserter{T}"/> which asserts that a value
    /// is before <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The reference value to check against.</param>
    /// <param name="boundsType">
    /// Whether <paramref name="value"/> itself is a valid value; defaults to it not being valid.
    /// </param>
    public static IAsserter<T> IsBefore<T>(T value, BoundsType boundsType = BoundsType.Exclusive) where T : IComparable<T>
    {
        var upperBound = new ComparisonBoundary<T>(value, boundsType is BoundsType.Inclusive);
        return new IsInRangeAsserter<T>(lowerBoundary: null, upperBoundary: upperBound);
    }

    /// <summary>
    /// Return a new instance of <see cref="IsInRangeAsserter{T}"/> which asserts that a value
    /// is after <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The reference value to check against.</param>
    /// <param name="boundsType">
    /// Whether <paramref name="value"/> itself is a valid value; defaults to it not being valid.
    /// </param>
    public static IAsserter<T> IsAfter<T>(T value, BoundsType boundsType = BoundsType.Exclusive) where T : IComparable<T>
    {
        var lowerBound = new ComparisonBoundary<T>(value, boundsType is BoundsType.Inclusive);
        return new IsInRangeAsserter<T>(lowerBoundary: lowerBound, upperBoundary: null);
    }

    /// <summary>
    /// Return a new instance of <see cref="IsInRangeAsserter{T}"/> which asserts that a value
    /// is between <paramref name="start"/> and <paramref name="end"/>.
    /// </summary>
    /// <param name="start">The lower bound of allowed values.</param>
    /// <param name="end">The upper bound of allowed values.</param>
    /// <param name="boundsType">
    /// Whether <paramref name="start"/> and <paramref name="end"/> itself are valid values; defaults to them not being valid.
    /// </param>
    public static IAsserter<T> IsBetween<T>(T start, T end, BoundsType boundsType = BoundsType.Exclusive) where T : IComparable<T>
    {
        return IsBetween(start, boundsType, end, boundsType);
    }

    /// <summary>
    /// Return a new instance of <see cref="IsInRangeAsserter{T}"/> which asserts that a value
    /// is between <paramref name="start"/> and <paramref name="end"/>.
    /// </summary>
    /// <param name="start">The lower bound of allowed values.</param>
    /// <param name="end">The upper bound of allowed values.</param>
    /// <param name="startBoundsType">
    /// Whether <paramref name="start"/> itself is a valid values; defaults to it not being valid.
    /// </param>
    /// <param name="endBoundsType">
    /// Whether <paramref name="end"/> itself is a valid values; defaults to it not being valid.
    /// </param>
    public static IAsserter<T> IsBetween<T>(T start, BoundsType startBoundsType, T end, BoundsType endBoundsType) where T : IComparable<T>
    {
        var lowerBound = new ComparisonBoundary<T>(start, startBoundsType is BoundsType.Inclusive);
        var upperBound = new ComparisonBoundary<T>(end, endBoundsType is BoundsType.Inclusive);

        return new IsInRangeAsserter<T>(lowerBoundary: lowerBound, upperBoundary: upperBound);
    }
}

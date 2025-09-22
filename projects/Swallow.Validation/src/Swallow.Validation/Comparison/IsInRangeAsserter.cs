namespace Swallow.Validation.V2.Comparison;

/// <summary>
/// An error signaling that a value was outside of a given range.
/// </summary>
/// <param name="lowerBoundary">The lower boundary for allowed values, if any.</param>
/// <param name="upperBoundary">The upper boundary for allowed values, if any.</param>
public sealed class NotInRange<T>(ComparisonBoundary<T>? lowerBoundary = null, ComparisonBoundary<T>? upperBoundary = null) : ValidationError where T : IComparable<T>
{
    /// <summary>
    /// The lower boundary.
    /// </summary>
    public ComparisonBoundary<T>? LowerBound { get; } = lowerBoundary;

    /// <summary>
    /// The upper boundary.
    /// </summary>
    public ComparisonBoundary<T>? UpperBound { get; } = upperBoundary;

    /// <inheritdoc />
    public override string Message => "be in range";
}

/// <summary>
/// Represents a boundary against which a value may be checked.
/// </summary>
/// <param name="Value">The boundary value itself.</param>
/// <param name="IsInclusive">Whether <paramref name="Value"/> is an acceptable value or not.</param>
/// <typeparam name="T">Type of value that is being compared.</typeparam>
public readonly record struct ComparisonBoundary<T>(T Value, bool IsInclusive) where T : IComparable<T>;

/// <summary>
/// How to match a value to a bound.
/// </summary>
public enum BoundsType
{
    /// <summary>
    /// The boundary value itself is <em>not</em> a valid value.
    /// </summary>
    Exclusive,

    /// <summary>
    /// The boundary value itself <em>is</em> a valid value.
    /// </summary>
    Inclusive
}

/// <summary>
/// An asserter to check that a value is within a given range; produces a
/// <see cref="NotInRange{T}"/> as validation error.
/// </summary>
/// <param name="lowerBoundary">The lower boundary for allowed values, if any.</param>
/// <param name="upperBoundary">The upper boundary for allowed values, if any.</param>
public sealed class IsInRangeAsserter<T>(ComparisonBoundary<T>? lowerBoundary = null, ComparisonBoundary<T>? upperBoundary = null) : IAsserter<T> where T : IComparable<T>
{
    /// <inheritdoc />
    public bool IsValid(T value)
    {
        return MatchesLowerBound(value) && MatchesUpperBound(value);
    }

    /// <inheritdoc />
    public ValidationError Error { get; } = new NotInRange<T>(lowerBoundary, upperBoundary);

    private bool MatchesLowerBound(T value)
    {
        return lowerBoundary switch
        {
            null => true,
            { IsInclusive: true, Value: var inclusiveLimit } => value.CompareTo(inclusiveLimit) >= 0,
            { IsInclusive: false, Value: var exclusiveLimit } => value.CompareTo(exclusiveLimit) > 0
        };
    }

    private bool MatchesUpperBound(T value)
    {
        return upperBoundary switch
        {
            null => true,
            { IsInclusive: true, Value: var inclusiveLimit } => value.CompareTo(inclusiveLimit) <= 0,
            { IsInclusive: false, Value: var exclusiveLimit } => value.CompareTo(exclusiveLimit) < 0
        };
    }
}

/// <summary>
/// Convenience functions to create instances of <see cref="IsInRangeAsserter{T}"/>.
/// </summary>
public static class IsInRangeAsserter
{
    /// <summary>
    /// Return a new instance of <see cref="IsInRangeAsserter{T}"/> which asserts that a value
    /// is before <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The reference value to check against.</param>
    /// <param name="boundsType">
    /// Whether <paramref name="value"/> itself is a valid value; defaults to it not being valid.
    /// </param>
    /// <returns>A constructed asserter.</returns>
    public static IsInRangeAsserter<T> Before<T>(T value, BoundsType boundsType = BoundsType.Exclusive) where T : IComparable<T>
    {
        var boundary = new ComparisonBoundary<T>(value, boundsType is BoundsType.Inclusive);
        return new IsInRangeAsserter<T>(upperBoundary: boundary);
    }

    /// <summary>
    /// Return a new instance of <see cref="IsInRangeAsserter{T}"/> which asserts that a value
    /// is after <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The reference value to check against.</param>
    /// <param name="boundsType">
    /// Whether <paramref name="value"/> itself is a valid value; defaults to it not being valid.
    /// </param>
    /// <returns>A constructed asserter.</returns>
    public static IsInRangeAsserter<T> After<T>(T value, BoundsType boundsType = BoundsType.Exclusive) where T : IComparable<T>
    {
        var boundary = new ComparisonBoundary<T>(value, boundsType is BoundsType.Inclusive);
        return new IsInRangeAsserter<T>(lowerBoundary: boundary);
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
    /// <returns>A constructed asserter.</returns>
    public static IsInRangeAsserter<T> Between<T>(T start, T end, BoundsType boundsType = BoundsType.Exclusive) where T : IComparable<T>
    {
        return Between(start: start, end: end, startBoundsType: boundsType, endBoundsType: boundsType);
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
    /// <returns>A constructed asserter.</returns>
    public static IsInRangeAsserter<T> Between<T>(T start, T end, BoundsType startBoundsType, BoundsType endBoundsType) where T : IComparable<T>
    {
        var lowerBoundary = new ComparisonBoundary<T>(start, startBoundsType is BoundsType.Inclusive);
        var upperBoundary = new ComparisonBoundary<T>(end, endBoundsType is BoundsType.Inclusive);

        return new IsInRangeAsserter<T>(lowerBoundary: lowerBoundary, upperBoundary: upperBoundary);
    }
}

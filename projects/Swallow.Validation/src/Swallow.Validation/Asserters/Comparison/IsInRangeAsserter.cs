namespace Swallow.Validation.Next.Asserters.Comparison;

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

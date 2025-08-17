#nullable enable
using System.Diagnostics.CodeAnalysis;
using Swallow.Validation.Errors;

namespace Swallow.Validation.Assertions.Times;

/// <summary>
/// An error signalling that a <see cref="DateTime"/> was outside of a given range.
/// </summary>
/// <param name="lowerBound">The lower bound.</param>
/// <param name="upperBound">The upper bound.</param>
/// <param name="lowerBoundType">Whether the lower bound itself is a valid value.</param>
/// <param name="upperBoundType">Whether the upper bound itself is a valid value.</param>
public sealed class DateTimeNotInRange(
    DateTime? lowerBound,
    DateTime? upperBound,
    BoundsType lowerBoundType,
    BoundsType upperBoundType) : ValidationError
{
    /// <summary>
    /// The lower bound.
    /// </summary>
    public DateTime? LowerBound { get; } = lowerBound;

    /// <summary>
    /// The upper bound.
    /// </summary>
    public DateTime? UpperBound { get; } = upperBound;

    /// <summary>
    /// Whether the lower bound itself is a valid value.
    /// </summary>
    public BoundsType LowerBoundType { get; } = lowerBoundType;

    /// <summary>
    /// Whether the upper bound itself is a valid value.
    /// </summary>
    public BoundsType UpperBoundType { get; } = upperBoundType;

    /// <inheritdoc />
    public override string Message => $"{PropertyName} is outside of the valid range";
}

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
/// An asserter to check that a <see cref="DateTime"/> is within a given range; produces a
/// <see cref="DateTimeNotInRange"/> as validation error.
/// </summary>
/// <param name="lowerBound">The lower bound; exclusive by default.</param>
/// <param name="upperBound">The upper bound; exclusive by default.</param>
/// <param name="lowerBoundType">How to match the lower bound; defaults to exclusive.</param>
/// <param name="upperBoundType">How to match the upper bound; defaults to exclusive.</param>
public sealed class IsDateTimeInRangeAsserter(
    DateTime? lowerBound = null,
    DateTime? upperBound = null,
    BoundsType lowerBoundType = BoundsType.Exclusive,
    BoundsType upperBoundType = BoundsType.Exclusive) : IAsserter<DateTime>
{
    /// <inheritdoc />
    public bool Check(INamedValueProvider<DateTime> valueProvider, [NotNullWhen(false)] out ValidationError? error)
    {
        if (MatchesLowerBound(valueProvider.Value) && MatchesUpperBound(valueProvider.Value))
        {
            error = null;
            return true;
        }

        error = new DateTimeNotInRange(lowerBound, upperBound, lowerBoundType, upperBoundType);
        return false;
    }

    private bool MatchesLowerBound(DateTime dateTime)
    {
        if (lowerBound is null)
        {
            return true;
        }

        return lowerBoundType switch
        {
            BoundsType.Exclusive => dateTime > lowerBound.Value,
            BoundsType.Inclusive => dateTime >= lowerBound.Value,
            _ => throw new ArgumentOutOfRangeException(paramName: nameof(lowerBoundType), actualValue: lowerBoundType, message: null)
        };
    }

    private bool MatchesUpperBound(DateTime dateTime)
    {
        if (upperBound is null)
        {
            return true;
        }

        return upperBoundType switch
        {
            BoundsType.Exclusive => dateTime < upperBound.Value,
            BoundsType.Inclusive => dateTime <= upperBound.Value,
            _ => throw new ArgumentOutOfRangeException(paramName: nameof(upperBoundType), actualValue: upperBoundType, message: null)
        };
    }
}

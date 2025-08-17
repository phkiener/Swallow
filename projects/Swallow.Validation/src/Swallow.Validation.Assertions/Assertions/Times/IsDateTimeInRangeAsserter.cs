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
/// <remarks>
/// Passing in <c>null</c> for both <paramref name="lowerBound"/> and <paramref name="upperBound"/>
/// is allowed, but it's not useful. The asserter will juts pass for any value.
/// </remarks>
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

    /// <summary>
    /// Return a new instance of <see cref="IsDateTimeInRangeAsserter"/> which asserts that a value
    /// is before <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The reference value to check against.</param>
    /// <param name="boundsType">
    /// Whether <paramref name="value"/> itself is a valid value; defaults to it not being valid.
    /// </param>
    /// <returns>A constructed asserter.</returns>
    public static IsDateTimeInRangeAsserter Before(DateTime value, BoundsType boundsType = BoundsType.Exclusive)
    {
        return new IsDateTimeInRangeAsserter(upperBound: value, upperBoundType: boundsType);
    }

    /// <summary>
    /// Return a new instance of <see cref="IsDateTimeInRangeAsserter"/> which asserts that a value
    /// is after <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The reference value to check against.</param>
    /// <param name="boundsType">
    /// Whether <paramref name="value"/> itself is a valid value; defaults to it not being valid.
    /// </param>
    /// <returns>A constructed asserter.</returns>
    public static IsDateTimeInRangeAsserter After(DateTime value, BoundsType boundsType = BoundsType.Exclusive)
    {
        return new IsDateTimeInRangeAsserter(lowerBound: value, lowerBoundType: boundsType);
    }

    /// <summary>
    /// Return a new instance of <see cref="IsDateTimeInRangeAsserter"/> which asserts that a value
    /// is between <paramref name="start"/> and <paramref name="end"/>.
    /// </summary>
    /// <param name="start">The lower bound of allowed values.</param>
    /// <param name="end">The upper bound of allowed values.</param>
    /// <param name="boundsType">
    /// Whether <paramref name="start"/> and <paramref name="end"/> itself are valid values; defaults to them not being valid.
    /// </param>
    /// <returns>A constructed asserter.</returns>
    public static IsDateTimeInRangeAsserter Between(DateTime start, DateTime end, BoundsType boundsType = BoundsType.Exclusive)
    {
        return Between(start: start, end: end, startBoundsType: boundsType, endBoundsType: boundsType);
    }

    /// <summary>
    /// Return a new instance of <see cref="IsDateTimeInRangeAsserter"/> which asserts that a value
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
    public static IsDateTimeInRangeAsserter Between(DateTime start, DateTime end, BoundsType startBoundsType, BoundsType endBoundsType)
    {
        return new IsDateTimeInRangeAsserter(lowerBound: start, upperBound: end, lowerBoundType: startBoundsType, upperBoundType: endBoundsType);
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

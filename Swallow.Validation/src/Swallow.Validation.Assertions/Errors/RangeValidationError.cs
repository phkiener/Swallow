namespace Swallow.Validation.Errors;

using System;

/// <summary>
///     A validation error for a value being out of a certain range.
/// </summary>
/// <typeparam name="T">Type of the value.</typeparam>
public sealed class RangeValidationError<T> : ValidationError where T : IComparable<T>
{
    private readonly bool lowerBoundSpecified;
    private readonly bool lowerIsInclusive;

    private readonly bool upperBoundSpecified;
    private readonly bool upperIsInclusive;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RangeValidationError{T}" /> class.
    /// </summary>
    /// <param name="lowerBound">The lower bound that was not met, inclusive.</param>
    /// <param name="upperBound">The upper bound that was not met, inclusive.</param>
    public RangeValidationError(T lowerBound, T upperBound)
    {
        LowerBound = lowerBound;
        lowerBoundSpecified = true;
        lowerIsInclusive = true;
        UpperBound = upperBound;
        upperBoundSpecified = true;
        upperIsInclusive = true;
    }

    private RangeValidationError(
        T lowerBound,
        bool hasLowerBound,
        bool lowerBoundIsInclusive,
        T upperBound,
        bool hasUpperBound,
        bool upperBoundIsInclusive)
    {
        LowerBound = lowerBound;
        UpperBound = upperBound;
        lowerBoundSpecified = hasLowerBound;
        upperBoundSpecified = hasUpperBound;
        lowerIsInclusive = lowerBoundIsInclusive;
        upperIsInclusive = upperBoundIsInclusive;
    }

    /// <summary>Gets the lower bound to satisfy, inclusive.</summary>
    public T LowerBound { get; }

    /// <summary>Gets the upper bound to satisfy, inclusive.</summary>
    public T UpperBound { get; }

    /// <inheritdoc />
    public override string Message => BuildMessage();

    /// <summary>
    ///     Creates a new instance of the <see cref="RangeValidationError{T}" /> class.
    /// </summary>
    /// <param name="value">The lower bound that was not met</param>
    /// <param name="isInclusive">Whether the lower bound is inclusive (greater than or equal) or not</param>
    /// <returns>A range validation error with a set lower bound and no upper bound</returns>
    public static RangeValidationError<T> FromLowerBound(T value, bool isInclusive)
    {
        return new(
            lowerBound: value,
            hasLowerBound: true,
            lowerBoundIsInclusive: isInclusive,
            upperBound: default,
            hasUpperBound: false,
            upperBoundIsInclusive: false);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="RangeValidationError{T}" /> class.
    /// </summary>
    /// <param name="value">The upper bound that was not met, inclusive</param>
    /// <param name="isInclusive">Whether the upper bound is inclusive (less than or equal) or not</param>
    /// <returns>A range validation error with a set upper bound and no lower bound</returns>
    public static RangeValidationError<T> FromUpperBound(T value, bool isInclusive)
    {
        return new(
            lowerBound: default,
            hasLowerBound: false,
            lowerBoundIsInclusive: false,
            upperBound: value,
            hasUpperBound: true,
            upperBoundIsInclusive: isInclusive);
    }

    private string BuildMessage()
    {
        var rangeSpecification = this switch
        {
            { lowerBoundSpecified: true, upperBoundSpecified: true } => $"between {LowerBound} and {UpperBound}",
            { lowerBoundSpecified: true } => $"greater than {(lowerIsInclusive ? "or equal to " : "")}{LowerBound}",
            { upperBoundSpecified: true } => $"less than {(upperIsInclusive ? "or equal to " : "")}{UpperBound}",
            _ => throw new InvalidOperationException($"Either {nameof(LowerBound)} or {nameof(UpperBound)} must be specified")
        };

        return $"{PropertyName} must be {rangeSpecification}, but was {ActualValue}";
    }
}

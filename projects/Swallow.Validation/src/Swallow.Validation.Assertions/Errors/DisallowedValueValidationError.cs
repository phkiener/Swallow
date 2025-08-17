namespace Swallow.Validation.Errors;

using System.Collections.Generic;

/// <summary>
///     A validation error in case a value is not part of an allowed set of values.
/// </summary>
/// <typeparam name="T">Type of the value checked.</typeparam>
public sealed class DisallowedValueValidationError<T> : ValidationError
{
    private DisallowedValueValidationError(ICollection<T> values, bool shouldNotMatch)
    {
        Values = values;
        ShouldNotMatch = shouldNotMatch;
    }

    /// <summary>
    ///     Gets the collection of allowed values.
    /// </summary>
    public ICollection<T> Values { get; }

    /// <summary>
    ///     Gets whether the value should not be contained in the collection of allowed values.
    /// </summary>
    public bool ShouldNotMatch { get; }

    /// <inheritdoc />
    public override string Message
        => $"{PropertyName} {(ShouldNotMatch ? "may not" : "must")} be in ({string.Join(separator: ", ", values: Values)}) but was {ActualValue}";

    /// <summary>
    ///     Create a new error signaling that a value must be contained in a certain set of values.
    /// </summary>
    /// <param name="values">The values that are allowed.</param>
    /// <returns>The error.</returns>
    public static DisallowedValueValidationError<T> BeOneOf(ICollection<T> values)
    {
        return new(values: values, shouldNotMatch: false);
    }

    /// <summary>
    ///     Create a new error signaling that a value may not be contained in a certain set of values.
    /// </summary>
    /// <param name="values">The values that are not allowed.</param>
    /// <returns>The error.</returns>
    public static DisallowedValueValidationError<T> NotBeOneOf(ICollection<T> values)
    {
        return new(values: values, shouldNotMatch: true);
    }
}

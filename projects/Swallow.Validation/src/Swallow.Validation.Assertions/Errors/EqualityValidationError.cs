namespace Swallow.Validation.Errors;

/// <summary>
///     An error to signal required/forbidden equality.
/// </summary>
/// <remarks>
///     This can be used to validate that two property sets to be merged refer to the same
///     entity, for example.
/// </remarks>
/// <typeparam name="T">Type of the expected value.</typeparam>
public sealed class EqualityValidationError<T> : ValidationError
{
    private EqualityValidationError(T expectedValue, bool shouldNotBeEqual)
    {
        ExpectedValue = expectedValue;
        ShouldNotBeEqual = shouldNotBeEqual;
    }

    /// <summary>
    ///     Gets the expected (or forbidden) value.
    /// </summary>
    public T ExpectedValue { get; }

    /// <summary>
    ///     Gets whether equality is forbidden instead of required.
    /// </summary>
    public bool ShouldNotBeEqual { get; }

    /// <inheritdoc />
    public override string Message
        => ShouldNotBeEqual ? $"{PropertyName} should not be {ExpectedValue}" : $"{PropertyName} should be {ExpectedValue} but was {ActualValue}";

    /// <summary>
    ///     Create a new error signaling that the checked value must be equal to the given value.
    /// </summary>
    /// <param name="value">The value that is required</param>
    /// <returns>The error.</returns>
    public static EqualityValidationError<T> MustBe(T value)
    {
        return new(expectedValue: value, shouldNotBeEqual: false);
    }

    /// <summary>
    ///     Create a new error signaling that the checked value must not be equal to the given value.
    /// </summary>
    /// <param name="value">The value that is forbidden</param>
    /// <returns>The error.</returns>
    public static EqualityValidationError<T> MustNotBe(T value)
    {
        return new(expectedValue: value, shouldNotBeEqual: true);
    }
}

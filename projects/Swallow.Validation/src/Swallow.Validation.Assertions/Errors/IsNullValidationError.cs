namespace Swallow.Validation.Errors;

/// <summary>
///     A validation error in case of a value being null when it is not supposed to.
/// </summary>
public sealed class IsNullValidationError : ValidationError
{
    /// <inheritdoc />
    public override string Message => $"{PropertyName} must not be null";
}

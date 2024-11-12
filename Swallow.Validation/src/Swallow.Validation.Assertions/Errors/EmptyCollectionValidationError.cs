namespace Swallow.Validation.Errors;

/// <summary>
///     A validation error in case of a collection being empty.
/// </summary>
public sealed class EmptyCollectionValidationError : ValidationError
{
    /// <inheritdoc />
    public override string Message => $"{PropertyName} must not be empty";
}

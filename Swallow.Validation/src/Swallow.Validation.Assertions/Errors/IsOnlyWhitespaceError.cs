namespace Swallow.Validation.Errors;

/// <summary>
///     A validation error in case of a string consisting of only whitespace characters.
/// </summary>
public sealed class IsOnlyWhitespaceError : ValidationError
{
    /// <inheritdoc />
    public override string Message => $"{PropertyName} must contain at least one non-whitespace character.";
}

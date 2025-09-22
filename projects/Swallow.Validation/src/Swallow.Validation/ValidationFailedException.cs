namespace Swallow.Validation.Next;

/// <summary>
/// An <see cref="Exception"/> signalling that validation has failed.
/// </summary>
/// <param name="errors">The validation errors that have been encountered.</param>
public sealed class ValidationFailedException(IEnumerable<ValidationError> errors) : Exception("Validation failed")
{
    /// <summary>
    /// All validation errors that have been encountered.
    /// </summary>
    public IReadOnlyList<ValidationError> ValidationErrors { get; } = errors.ToArray();
}

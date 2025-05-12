namespace Swallow.Validation;

/// <summary>
///     Interface for all classes that may be validated on their own.
/// </summary>
public interface IValidatable
{
    /// <summary>
    ///     Validate the entity, returning a validation result.
    /// </summary>
    /// <returns>A validation result containing errors - or not.</returns>
    ValidationResult Validate();
}

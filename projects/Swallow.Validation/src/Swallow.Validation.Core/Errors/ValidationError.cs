namespace Swallow.Validation.Errors;

/// <summary>
///     The base for any type of validation error.
/// </summary>
/// <remarks>
///     Every validation error contains at least the name of the property
///     that failed validation.
///     <para>
///         If you want to create your own validation error with certain fields, simply
///         inherit from <c>ValidationError</c>. The relevant field, the property name,
///         will be automatically injected as part of generating the error in a validation
///         rule.
///     </para>
/// </remarks>
public abstract class ValidationError
{
    /// <summary>
    ///     Gets the name of the property that failed validation.
    /// </summary>
    public string PropertyName { get; internal set; } = null!;

    /// <summary>
    ///     Gets a string representation of the value that failed validation.
    /// </summary>
    public string ActualValue { get; internal set; } = "null";

    /// <summary>
    ///     Gets the message of the validation error.
    /// </summary>
    public abstract string Message { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        return Message;
    }
}

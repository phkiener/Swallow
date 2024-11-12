namespace Swallow.Validation;

using Errors;

/// <summary>
///     An easier way to implement a custom asserter.
/// </summary>
/// <typeparam name="T">The type of the asserter value.</typeparam>
public abstract class SimpleAsserter<T> : IAsserter<T>
{
    /// <summary>
    ///     Name of the asserted value.
    /// </summary>
    protected string PropertyName { get; private set; } = null!;

    /// <inheritdoc />
    public bool Check(INamedValueProvider<T> valueProvider, out ValidationError error)
    {
        PropertyName = valueProvider.Name;
        var result = Validate(valueProvider.Value);
        error = result.IsSuccess ? null : new EntityValidationError(result.Errors);

        return result.IsSuccess;
    }

    /// <summary>
    ///     Validate the given value, producing a validation result.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>A validation result for that value.</returns>
    protected abstract ValidationResult Validate(T value);
}

namespace Swallow.Validation.Validatable;

using Errors;

/// <summary>
///     An asserter using the <see cref="IValidatable.Validate()" /> method.
/// </summary>
/// <typeparam name="T">Type of the object to validate.</typeparam>
public class ValidatableEntityAsserter<T> : IAsserter<T> where T : IValidatable
{
    /// <inheritdoc />
    public bool Check(INamedValueProvider<T> valueProvider, out ValidationError error)
    {
        var result = valueProvider.Value.Validate();
        error = result.IsSuccess ? null : new EntityValidationError(result.Errors);

        return error == null;
    }
}

namespace Swallow.Validation;

using Errors;

/// <summary>
///     An asserter to validate a condition.
/// </summary>
/// <typeparam name="TValue">Type of the validated value.</typeparam>
public interface IAsserter<in TValue>
{
    /// <summary>
    ///     Check that the given value satisfies the condition.
    /// </summary>
    /// <param name="valueProvider">Provider yielding the value to check.</param>
    /// <param name="error">The constructed error if the validation fails or <c>null</c></param>
    /// <returns><c>true</c> if the validation succeeds, <c>false</c> if an error is produced.</returns>
    /// <remarks>
    ///     <p>
    ///         Note to implementers: <br />
    ///         Even though the method is given an <see cref="INamedValueProvider{T}" />, you do not need to set the property name of a resulting
    ///         error yourself, as it is handled by the framework. If however you construct inner validation errors not directly returned, you do
    ///         need to make sure that the correct name is passed to these errors.
    ///     </p>
    /// </remarks>
    bool Check(INamedValueProvider<TValue> valueProvider, out ValidationError error);
}

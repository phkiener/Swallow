namespace Swallow.Validation.Assertions;

using Validatable;

/// <summary>
///     Extensions for validating values that cannot be grouped otherwise.
/// </summary>
public static class ValidatableEntityExtensions
{
    /// <summary>
    ///     Asserts that a given entity is valid using it's <see cref="IValidatable.Validate()" /> method.
    /// </summary>
    /// <param name="assertion">The assertion to use.</param>
    /// <typeparam name="T">Type of the validatable entity.</typeparam>
    /// <returns>The constructed assertion.</returns>
    public static IAssertion<T> IsValid<T>(this IAssertable<T> assertion) where T : IValidatable
    {
        return assertion.Satisfies(new ValidatableEntityAsserter<T>());
    }
}

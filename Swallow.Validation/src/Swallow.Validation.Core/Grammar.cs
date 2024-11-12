namespace Swallow.Validation;

using System;

/// <summary>
///     An object that can initiate an assertion on a certain value.
/// </summary>
public interface IValidation
{
    /// <summary>
    ///     Begin asserting a certain value.
    /// </summary>
    /// <param name="namedValue">The named value to assert.</param>
    /// <typeparam name="T">Type of the value to assert.</typeparam>
    /// <returns>An unformatted assertable for that value.</returns>
    IUnformattedAssertable<T> That<T>(INamedValueProvider<T> namedValue);

    /// <summary>
    ///     Finalizes the validation by producing a result.
    /// </summary>
    /// <returns>A validation result containing a list of validation errors - if any happened.</returns>
    ValidationResult Result();
}

/// <summary>
///     An assertable whose value will be default-formatted when shown in an error.
/// </summary>
/// <typeparam name="T">Type of the validated value.</typeparam>
/// <remarks>
///     By default, <c>ToString()</c> is called on the validated value when providing the
///     actual value to the validation error. If you wish to override this, you may call
///     <see cref="ShownAs" /> to provide a custom formatting function.
/// </remarks>
public interface IUnformattedAssertable<out T> : IAssertable<T>
{
    /// <summary>
    ///     Provide a display function for this assertable.
    /// </summary>
    /// <param name="displayFunction">The function to use for rendering the validated value.</param>
    /// <returns>A formatted assertable for the validated value.</returns>
    IAssertable<T> ShownAs(Func<T, string> displayFunction);
}

/// <summary>
///     An object that can check a certain value with a predicate.
/// </summary>
/// <typeparam name="T">Type of the asserted value.</typeparam>
/// <remarks>
///     To reuse predicates and error-building, it is encouraged to add extension methods to
///     IAssertable that call any of the Satisfies-methods with a predetermined predicate and
///     error-builder.
/// </remarks>
public interface IAssertable<out T>
{
    /// <summary>
    ///     Validate the checked value by using an <c>IAsserter</c>.
    /// </summary>
    /// <param name="asserter">The asserter to use.</param>
    /// <returns>An assertion to limit the validation to a certain condition, begin a new validation or finish the validation.</returns>
    IAssertion<T> Satisfies(IAsserter<T> asserter);
}

/// <summary>
///     An assertion of a value.
/// </summary>
/// <typeparam name="T">Type of the validated value.</typeparam>
public interface IAssertion<out T> : IValidation, IAssertable<T>, IConditionable<T>;

/// <summary>
///     An object to control the validation of a value, making sure it is processed only if a condition is met.
/// </summary>
public interface IConditionable<out T>
{
    /// <summary>
    ///     Make this validation only execute when the provided value is <c>true</c>.
    /// </summary>
    /// <param name="valueProvider">Provider for the value determining whether to validate or not.</param>
    /// <returns>A conditioned assertion to begin a new validation with.</returns>
    IConditionedAssertion<T> When(IValueProvider<bool> valueProvider);

    /// <summary>
    ///     Make this validation only execute when the provided value is <c>false</c>.
    /// </summary>
    /// <param name="valueProvider">Provider for the value determining whether to skip validation or not.</param>
    /// <returns>A conditioned assertion to begin a new validation with.</returns>
    IConditionedAssertion<T> Unless(IValueProvider<bool> valueProvider);
}

/// <summary>
///     An assertion that has had a condition attached to it.
/// </summary>
/// <typeparam name="T">Type of the validated value.</typeparam>
public interface IConditionedAssertion<out T> : IValidation, IAssertable<T>;

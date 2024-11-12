namespace Swallow.Validation;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Errors;
using Exceptions;
using Internal;

/// <summary>
///     Extension methods for <see cref="IValidation" /> allowing easier usage.
/// </summary>
public static class ValidationExtensions
{
    /// <summary>
    ///     Begin validating a certain value.
    /// </summary>
    /// <param name="validation">The validation from which to create the assertable.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="name">The name of the value.</param>
    /// <typeparam name="T">Type of the value to validate.</typeparam>
    /// <returns>An unformatted assertable for that value.</returns>
    public static IUnformattedAssertable<T> That<T>(this IValidation validation, T value, [CallerArgumentExpression("value")] string name = default)
    {
        return validation.That(new NamedStoredValue<T>(value: value, name: name));
    }

    /// <summary>
    ///     Begin validating a certain value that is given as a function.
    /// </summary>
    /// <param name="validation">The validation from which to create the assertable.</param>
    /// <param name="valueFunc">The value to validate.</param>
    /// <param name="name">The name of the value.</param>
    /// <typeparam name="T">Type of the value to validate.</typeparam>
    /// <returns>An unformatted assertable for that value.</returns>
    public static IUnformattedAssertable<T> That<T>(
        this IValidation validation,
        Func<T> valueFunc,
        [CallerArgumentExpression("valueFunc")] string name = default)
    {
        const string lambdaStart = "() => ";
        var cleanedName = name!.StartsWith(lambdaStart) ? name.Remove(startIndex: 0, count: lambdaStart.Length) : name;

        return validation.That(new NamedEvaluatedValue<T>(function: valueFunc, name: cleanedName));
    }

    /// <summary>
    ///     Finalizes the validation by producing a result.
    /// </summary>
    /// <param name="validation">The validation to act upon.</param>
    /// <param name="resultFunction">The function to produce the result if no validation errors occurred.</param>
    /// <typeparam name="TResult">Type of the result to produce.</typeparam>
    /// <returns>A validation result containing either a value on success or a list of validation errors.</returns>
    public static ValidationResult<TResult> Then<TResult>(this IValidation validation, Func<TResult> resultFunction)
    {
        var result = validation.Result();

        return result.IsSuccess ? ValidationResult.Success(resultFunction.Invoke()) : ValidationResult<TResult>.Failure(result.Errors);
    }

    /// <summary>
    ///     Finalizes the validation and executes an action if any errors happened.
    /// </summary>
    /// <param name="validation">The validation to act upon.</param>
    /// <param name="action">The action to execute in case of validation errors.</param>
    public static void Else(this IValidation validation, Action<IEnumerable<ValidationError>> action)
    {
        var result = validation.Result();
        if (result.IsError)
        {
            action(result.Errors);
        }
    }

    /// <summary>
    ///     Finalizes the validation by throwing an exception if any errors happened.
    /// </summary>
    /// <exception cref="ValidationException">If there are any validation errors.</exception>
    public static void ElseThrow(this IValidation validation)
    {
        validation.Result().ThrowErrors();
    }
}

/// <summary>
///     Extension methods for <see cref="IAssertable{T}" />s.
/// </summary>
public static class AssertableExtensions
{
    /// <summary>
    ///     Validate the checked value by applying a predicate to it.
    /// </summary>
    /// <param name="assertable">The assertable to act upon.</param>
    /// <param name="predicate">The predicate to validate.</param>
    /// <param name="errorFunc">A function to create an error if the predicate is not satisfied.</param>
    /// <typeparam name="TValue">Type of the validated value.</typeparam>
    /// <typeparam name="TError">Type of the validation error.</typeparam>
    /// <returns>The built assertion.</returns>
    public static IAssertion<TValue> Satisfies<TValue, TError>(
        this IAssertable<TValue> assertable,
        Func<TValue, bool> predicate,
        Func<TValue, TError> errorFunc) where TError : ValidationError
    {
        var asserter = new FunctionAsserter<TValue>(predicate: predicate, errorFunc: errorFunc);

        return assertable.Satisfies(asserter);
    }

    /// <summary>
    ///     Validate the checked value by applying a predicate to it.
    /// </summary>
    /// <remarks>
    ///     The message given should be like "be foo", as the constructed error message will follow the layout
    ///     <c>$"{name} must {message} but was {actual}"</c>.
    /// </remarks>
    /// <param name="assertable">The assertable to act upon.</param>
    /// <param name="predicate">The predicate to validate.</param>
    /// <param name="message">Message describing the required state for a validation error.</param>
    /// <returns>The built assertion.</returns>
    public static IAssertion<T> Satisfies<T>(this IAssertable<T> assertable, Func<T, bool> predicate, string message = null)
    {
        return assertable.Satisfies(predicate: predicate, errorFunc: _ => new GenericValidationError(message));
    }
}

/// <summary>
///     Extension methods for <see cref="IConditionable{T}" /> allowing easier usage.
/// </summary>
public static class ConditionableExtensions
{
    /// <summary>
    ///     Make the current validation only execute when the condition is met.
    /// </summary>
    /// <param name="conditionable">The conditionable for which to add the condition.</param>
    /// <param name="condition">A boolean to determine if the condition is met.</param>
    /// <returns>A conditioned assertion which cannot have another condition set.</returns>
    public static IConditionedAssertion<T> When<T>(this IConditionable<T> conditionable, bool condition)
    {
        return conditionable.When(new StoredValue<bool>(condition));
    }

    /// <summary>
    ///     Make the current validation only execute when the condition is met.
    /// </summary>
    /// <param name="conditionable">The conditionable for which to add the condition.</param>
    /// <param name="condition">A function to determine if the condition is met.</param>
    /// <returns>A conditioned assertion which cannot have another condition set.</returns>
    public static IConditionedAssertion<T> When<T>(this IConditionable<T> conditionable, Func<bool> condition)
    {
        return conditionable.When(new EvaluatedValue<bool>(condition));
    }

    /// <summary>
    ///     Make the current validation only execute when the condition is not met.
    /// </summary>
    /// <param name="conditionable">The conditionable for which to add the condition.</param>
    /// <param name="condition">A boolean to determine if the condition is met.</param>
    /// <returns>A conditioned assertion which cannot have another condition set.</returns>
    public static IConditionedAssertion<T> Unless<T>(this IConditionable<T> conditionable, bool condition)
    {
        return conditionable.Unless(new StoredValue<bool>(condition));
    }

    /// <summary>
    ///     Make the current validation only execute when the condition is not met.
    /// </summary>
    /// <param name="conditionable">The conditionable for which to add the condition.</param>
    /// <param name="condition">A function to determine if the condition is met.</param>
    /// <returns>A conditioned assertion which cannot have another condition set.</returns>
    public static IConditionedAssertion<T> Unless<T>(this IConditionable<T> conditionable, Func<bool> condition)
    {
        return conditionable.Unless(new EvaluatedValue<bool>(condition));
    }
}

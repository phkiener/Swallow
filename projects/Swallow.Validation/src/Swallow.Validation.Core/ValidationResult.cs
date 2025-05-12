namespace Swallow.Validation;

using System;
using System.Collections.Generic;
using System.Linq;
using Errors;
using Exceptions;

/// <summary>
///     A validation result containing a list of errors - if any happened.
/// </summary>
public class ValidationResult
{
    /// <summary>
    ///     Construct a validation result with a list of errors.
    /// </summary>
    /// <param name="errors">The errors for this validation result.</param>
    /// <remarks>
    ///     An empty list of errors signals a successful validation.
    /// </remarks>
    internal ValidationResult(IReadOnlyList<ValidationError> errors)
    {
        Errors = errors;
    }

    /// <summary>Gets a value indicating whether the validation succeeded.</summary>
    public bool IsSuccess => !Errors.Any();

    /// <summary>Gets a value indicating whether the validation failed.</summary>
    public bool IsError => Errors.Any();

    /// <summary>Gets a list of all validation errors that occurred.</summary>
    public IReadOnlyList<ValidationError> Errors { get; }

    /// <summary>
    ///     Create a validation result indicating success.
    /// </summary>
    /// <returns>The created validation result containing the given value.</returns>
    public static ValidationResult<TValue> Success<TValue>(TValue value)
    {
        return ValidationResult<TValue>.Success(value);
    }

    /// <summary>
    ///     Throws an exception if there are any validation errors.
    /// </summary>
    /// <exception cref="ValidationException">If there are any validation errors.</exception>
    public void ThrowErrors()
    {
        if (!IsSuccess)
        {
            throw new ValidationException(Errors);
        }
    }
}

/// <summary>
///     A validation result containing either a value or a list of errors.
/// </summary>
/// <typeparam name="T">Type of the success-value.</typeparam>
public sealed class ValidationResult<T> : ValidationResult
{
    private readonly T value;

    private ValidationResult(T value, IReadOnlyList<ValidationError> errors) : base(errors)
    {
        this.value = value;
    }

    /// <summary>Gets the value of this result.</summary>
    /// <exception cref="Exception">If the result contains validation errors.</exception>
    public T Value
    {
        get
        {
            ThrowErrors();

            return value!;
        }
    }

    /// <summary>
    ///     Create a validation result indicating success.
    /// </summary>
    /// <returns>The created validation result containing the given value.</returns>
    public static ValidationResult<T> Success(T value)
    {
        return new(value: value, errors: Array.Empty<ValidationError>());
    }

    /// <summary>
    ///     Create a validation result containing the given list of errors.
    /// </summary>
    /// <returns>The created validation result.</returns>
    public static ValidationResult<T> Failure(IEnumerable<ValidationError> errors)
    {
        return new(value: default, errors: errors.ToList());
    }
}

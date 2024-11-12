namespace Swallow.Validation;

/// <summary>
///     How to handle a failing validation.
/// </summary>
public enum FailureHandling
{
    /// <summary>
    ///     Always execute all validations.
    /// </summary>
    KeepGoing = 0,

    /// <summary>
    ///     Skip all remaining validations on a validation error.
    /// </summary>
    SkipAllRemaining = 1,

    /// <summary>
    ///     Skip all remaining validations for the same property on a validation error.
    /// </summary>
    SkipAllForProperty = 2
}

/// <summary>
///     The configuration for a validation spanning multiple assertions.
/// </summary>
public sealed class ValidatorConfiguration
{
    /// <summary>
    ///     If this is <c>true</c>, all validation rules are executed only when required by <see cref="IValidation.Result" />.
    /// </summary>
    /// <remarks>
    ///     Lazy validation means that all expressions, functions and predicates are only evaluated once they are
    ///     queried by asking for the result of the validation. Otherwise, a rule will be evaluated once it is
    ///     completely specified - either by asking for the result, beginning a new assertion or adding a new rule
    ///     to the current assertion.
    /// </remarks>
    public bool ValidateLazily { get; init; }

    /// <summary>
    ///     If this is <c>true</c>, any exceptions that happen while evaluating validation rules are caught.
    /// </summary>
    /// <remarks>
    ///     Caught exceptions cause the validation to fail with an exception validation error. If they are not to
    ///     be caught, they just bubble up to the calling code.
    /// </remarks>
    public bool CatchExceptions { get; init; } = true;

    /// <summary>
    ///     This sets the handling of failures for validation rules.
    /// </summary>
    public FailureHandling FailureHandling { get; init; } = FailureHandling.SkipAllForProperty;

    /// <summary>
    ///     Returns the default configuration for all validators.
    /// </summary>
    public static ValidatorConfiguration Default => new();
}

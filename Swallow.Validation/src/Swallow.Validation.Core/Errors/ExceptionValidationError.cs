namespace Swallow.Validation.Errors;

using System;

/// <summary>
///     A validation error that happens when an exception occurs while validating.
/// </summary>
public sealed class ExceptionValidationError : ValidationError
{
    /// <summary>
    ///     Create an exception validation error using the given exception as root cause.
    /// </summary>
    /// <param name="exception">Exception that is the cause for this validation error.</param>
    public ExceptionValidationError(Exception exception)
    {
        Exception = exception;
    }

    /// <summary>
    ///     Gets the exception that happened while validating.
    /// </summary>
    public Exception Exception { get; }

    /// <inheritdoc />
    public override string Message => $"{Exception.GetType().FullName}: {Exception.Message}";
}

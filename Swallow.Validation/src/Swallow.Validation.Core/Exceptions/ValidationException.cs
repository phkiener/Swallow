namespace Swallow.Validation.Exceptions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Errors;

/// <summary>
///     An exception signalling a failed validation.
/// </summary>
/// <remarks>
///     This exception is thrown when asking a validation result to do so. It does not happen when
///     an exception happens in the process of validating a value.
/// </remarks>
public sealed class ValidationException : Exception
{
    /// <summary>
    ///     Create a validation exception with a default message.
    /// </summary>
    /// <param name="errors">The validation errors that cause this exception.</param>
    public ValidationException(IEnumerable<ValidationError> errors)
    {
        Errors = errors.ToList();
    }

    /// <summary>
    ///     Returns a list of validation errors that caused this exception.
    /// </summary>
    public IReadOnlyList<ValidationError> Errors { get; }

    /// <inheritdoc />
    public override string Message => BuildMessage();

    private string BuildMessage()
    {
        var builder = new StringBuilder();
        builder.AppendLine("Validation failed");
        var current = 1;
        var count = Errors.Count;
        foreach (var error in Errors)
        {
            builder.AppendLine($"({current++}/{count}) {error}");
        }

        return builder.ToString().Trim();
    }
}

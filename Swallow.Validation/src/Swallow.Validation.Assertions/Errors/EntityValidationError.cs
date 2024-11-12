namespace Swallow.Validation.Errors;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
///     A validation error that groups validation errors under a common name.
/// </summary>
/// <remarks>
///     This error is typically used to collect multiple errors that happen on the same
///     entity (or POCO).
/// </remarks>
public sealed class EntityValidationError : ValidationError
{
    private const int InnerMessageIndentation = 2;

    /// <summary>
    ///     Initializes an instance of the <see cref="EntityValidationError" /> class
    /// </summary>
    /// <param name="validationErrors">A collection of validation errors.</param>
    public EntityValidationError(IEnumerable<ValidationError> validationErrors)
    {
        ValidationErrors = validationErrors.ToList();
    }

    /// <summary>
    ///     Returns a list of all errors that happened.
    /// </summary>
    public IReadOnlyList<ValidationError> ValidationErrors { get; }

    /// <inheritdoc />
    public override string Message => BuildMessage();

    private string BuildMessage()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"{PropertyName} failed validation");
        var current = 1;
        var count = ValidationErrors.Count;
        foreach (var error in ValidationErrors)
        {
            var lines = error.Message.Split(Environment.NewLine);
            var innerErrorIndex = $"({current++}/{count})";
            lines[0] = $"{innerErrorIndex} {lines[0]}";
            var indentedLines = lines.Select(line => new string(c: ' ', count: InnerMessageIndentation) + line);
            var indentedMessage = string.Join(separator: Environment.NewLine, values: indentedLines);
            builder.AppendLine(indentedMessage);
        }

        return builder.ToString().Trim();
    }
}

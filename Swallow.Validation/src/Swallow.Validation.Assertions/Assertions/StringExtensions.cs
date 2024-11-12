namespace Swallow.Validation.Assertions;

using System;
using System.Text.RegularExpressions;
using Errors;

/// <summary>
///     Extensions for validating strings in a shorter, prettier way.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    ///     Checks that a string matches a given regex.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="regex">The regex to test with.</param>
    /// <param name="targetDescription">Description of the format that the regex tests for, e.g. 'number'.</param>
    /// <param name="regexOptions">Options for regex matching.</param>
    /// <returns>The constructed assertion.</returns>
    public static IAssertion<string> Matches(
        this IAssertable<string> assertion,
        string regex,
        string targetDescription = null,
        RegexOptions regexOptions = RegexOptions.None)
    {
        return assertion.Satisfies(
            predicate: x => Regex.IsMatch(input: x, pattern: regex, options: regexOptions, matchTimeout: TimeSpan.FromSeconds(5)),
            errorFunc: _ => new RegexValidationError(testedRegex: regex, targetDescription: targetDescription));
    }

    /// <summary>
    ///     Checks that a string matches a given regex.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="regex">The regex to test with.</param>
    /// <param name="targetDescription">Description of the format that the regex tests for, e.g. 'number'.</param>
    /// <returns>The constructed assertion.</returns>
    public static IAssertion<string> Matches(this IAssertable<string> assertion, Regex regex, string targetDescription = null)
    {
        return assertion.Satisfies(
            predicate: regex.IsMatch,
            errorFunc: _ => new RegexValidationError(testedRegex: regex.ToString(), targetDescription: targetDescription));
    }

    /// <summary>
    ///     Checks that a string is not empty.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <returns>The constructed assertion.</returns>
    public static IAssertion<string> IsNotEmpty(this IAssertable<string> assertion)
    {
        return assertion.Satisfies(predicate: s => s.Length > 0, errorFunc: _ => new EmptyCollectionValidationError());
    }

    /// <summary>
    ///     Checks that a string consists of something other than whitespace, e.g. letters or numbers.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <returns>The constructed assertion.</returns>
    public static IAssertion<string> IsNotOnlyWhitespace(this IAssertable<string> assertion)
    {
        return assertion.Satisfies(predicate: s => string.IsNullOrWhiteSpace(s) is false, errorFunc: _ => new IsOnlyWhitespaceError());
    }
}

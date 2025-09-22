using System.Text.RegularExpressions;
using Swallow.Validation.Next.Asserters.Text;

namespace Swallow.Validation.Next.Asserters;

partial class Satisfies
{
    /// <summary>
    /// Returns an <see cref="IsNotEmptyAsserter"/> that checks whether a string is empty.
    /// </summary>
    public static IAsserter<string> NotEmpty { get; } = new IsNotEmptyAsserter();

    /// <summary>
    /// Returns an <see cref="IsNotEmptyOrWhitespaceAsserter"/> that checks whether a string is empty or only whitespace.
    /// </summary>
    public static IAsserter<string> NotEmptyOrWhitespace { get; } = new IsNotEmptyOrWhitespaceAsserter();

    /// <summary>
    /// Returns an <see cref="MatchesRegexAsserter"/> that checks whether a string matches the given regular expression.
    /// </summary>
    /// <param name="regex">The regular expression to match.</param>
    public static IAsserter<string> Regex(string regex) => new MatchesRegexAsserter(new Regex(regex));

    /// <summary>
    /// Returns an <see cref="MatchesRegexAsserter"/> that checks whether a string matches the given regular expression.
    /// </summary>
    /// <param name="regex">The regular expression to match.</param>
    public static IAsserter<string> Regex(Regex regex) => new MatchesRegexAsserter(regex);
}

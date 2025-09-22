using System.Text.RegularExpressions;

namespace Swallow.Validation.V2.Text;

/// <summary>
/// An error signaling that the asserted string does not conform to an expected format.
/// </summary>
/// <param name="regex">The regex that the value must conform to.</param>
public sealed class DoesNotMatchRegex(string regex) : ValidationError
{
    /// <summary>
    /// The regex that the value must conform to.
    /// </summary>
    public string? Regex { get; } = regex;

    /// <inheritdoc />
    public override string Message => $"match {Regex}";
}

/// <summary>
/// An asserter to check that a value conforms to a given regular expression; produces
/// <see cref="DoesNotMatchRegex"/> as validation error.
/// </summary>
/// <param name="regex">The regex that the value must conform to.</param>
public sealed class MatchesRegexAsserter(Regex regex) : IAsserter<string>
{
    /// <summary>
    /// Create a new <see cref="MatchesRegexAsserter"/> by compiling <paramref name="regex"/> into
    /// a <see cref="Regex"/>.
    /// </summary>
    /// <param name="regex">The regex that the value must conform to.</param>
    public MatchesRegexAsserter(string regex) : this(new Regex(regex)) { }

    /// <inheritdoc />
    public bool IsValid(string value)
    {
        return regex.IsMatch(value);
    }

    /// <inheritdoc />
    public ValidationError Error { get; } = new DoesNotMatchRegex(regex.ToString());
}

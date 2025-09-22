using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Swallow.Validation.Next.Asserters.Text;

[TestFixture]
public sealed partial class MatchesRegexAsserterTest
{
    private const string RegexPattern = "^\\w+$";

    [Test]
    public void ReportsSuccess_WhenValueMatchesPattern()
    {
        Assert.That(Satisfies.Regex(RegexPattern).IsValid("hello"), Is.True);
        Assert.That(Satisfies.Regex(CompiledRegex()).IsValid("hello"), Is.True);
    }

    [Test]
    public void ReportsError_WhenValueDoesNotMatchPattern()
    {
        Assert.That(Satisfies.Regex(RegexPattern).IsValid("    "), Is.False);
        Assert.That(Satisfies.Regex(CompiledRegex()).IsValid("    "), Is.False);
    }

    [Test]
    public void ReportedError_HasCorrectMessage()
    {
        var typedError = Satisfies.Regex(RegexPattern).Error as DoesNotMatchRegex;
        Assert.That(typedError?.Regex, Is.EqualTo("^\\w+$"));
        Assert.That(typedError?.Message, Is.EqualTo("match ^\\w+$"));
    }

    [GeneratedRegex("^\\w+$")]
    private partial Regex CompiledRegex();
}

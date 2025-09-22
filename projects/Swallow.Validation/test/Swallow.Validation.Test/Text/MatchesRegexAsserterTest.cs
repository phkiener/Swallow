using NUnit.Framework;

namespace Swallow.Validation.V2.Text;

[TestFixture]
public sealed class MatchesRegexAsserterTest
{
    private static readonly MatchesRegexAsserter Asserter = new("^\\w+$");

    [Test]
    public void ReportsSuccess_WhenValueMatchesPattern()
    {
        Assert.That(Asserter.IsValid("hello"), Is.True);
    }

    [Test]
    public void ReportsError_WhenValueDoesNotMatchPattern()
    {
        Assert.That(Asserter.IsValid("    "), Is.False);
    }

    [Test]
    public void ReportedError_HasCorrectMessage()
    {
        var typedError = Asserter.Error as DoesNotMatchRegex;;
        Assert.That(typedError?.Regex, Is.EqualTo("^\\w+$"));
        Assert.That(typedError?.Message, Is.EqualTo("match ^\\w+$"));
    }
}

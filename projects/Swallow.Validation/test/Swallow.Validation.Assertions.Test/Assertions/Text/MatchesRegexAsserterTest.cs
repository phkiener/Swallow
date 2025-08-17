#nullable enable
using NUnit.Framework;
using Swallow.Validation.Utils;

namespace Swallow.Validation.Assertions.Text;

[TestFixture]
public sealed class MatchesRegexAsserterTest
{
    private static readonly MatchesRegexAsserter Asserter = new("^\\w+$");

    [Test]
    public void ReportsSuccess_WhenValueMatchesPattern()
    {
        var result = AssertionTester.Assert("hello", Asserter, out _);

        Assert.That(result, Is.True);
    }

    [Test]
    public void ReportsError_WhenValueDoesNotMatchPattern()
    {
        var result = AssertionTester.Assert("    ", Asserter, out _);

        Assert.That(result, Is.False);
    }

    [Test]
    public void ReportedError_HasCorrectMessage()
    {
        AssertionTester.Assert("    ", Asserter, out var error);

        var typedError = error as DoesNotMatchRegex;;
        Assert.That(typedError?.Regex, Is.EqualTo("^\\w+$"));
        Assert.That(typedError?.Message, Is.EqualTo("value does not match ^\\w+$"));
    }
}

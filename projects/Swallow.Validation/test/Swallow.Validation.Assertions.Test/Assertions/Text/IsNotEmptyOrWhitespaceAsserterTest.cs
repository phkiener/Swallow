#nullable enable
using NUnit.Framework;
using Swallow.Validation.Utils;

namespace Swallow.Validation.Assertions.Text;

[TestFixture]
public sealed class IsNotEmptyOrWhitespaceAsserterTest
{
    private static readonly IsNotEmptyOrWhitespaceAsserter Asserter = new();

    [Test]
    public void ReportsSuccess_WhenValueIsNotEmpty()
    {
        var result = AssertionTester.Assert("hello", Asserter, out _);

        Assert.That(result, Is.True);
    }

    [Test]
    public void ReportsError_WhenValueIsEmpty()
    {
        var emptyStringResult = AssertionTester.Assert("", Asserter, out _);
        var spaceStringResult = AssertionTester.Assert("   ", Asserter, out _);
        var mixedWhitespaceResult = AssertionTester.Assert("\n\r\t", Asserter, out _);

        Assert.That(emptyStringResult, Is.False);
        Assert.That(spaceStringResult, Is.False);
        Assert.That(mixedWhitespaceResult, Is.False);
    }

    [Test]
    public void ReportedError_HasCorrectMessage()
    {
        AssertionTester.Assert("", Asserter, out var error);

        var typedError = error as OnlyWhitespaceString;
        Assert.That(typedError?.Message, Is.EqualTo("value contains only whitespace characters"));
    }
}

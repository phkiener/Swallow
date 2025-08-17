#nullable enable
using NUnit.Framework;
using Swallow.Validation.Utils;

namespace Swallow.Validation.Assertions.Text;

[TestFixture]
public sealed class IsNotEmptyAsserterTest
{
    private static readonly IsNotEmptyAsserter Asserter = new();

    [Test]
    public void ReportsSuccess_WhenValueIsNotEmpty()
    {
        var result = AssertionTester.Assert("hello", Asserter, out _);

        Assert.That(result, Is.True);
    }

    [Test]
    public void ReportsError_WhenValueIsEmpty()
    {
        var result = AssertionTester.Assert("", Asserter, out _);

        Assert.That(result, Is.False);
    }

    [Test]
    public void ReportedError_HasCorrectMessage()
    {
        AssertionTester.Assert("", Asserter, out var error);

        var typedError = error as EmptyString;
        Assert.That(typedError?.Message, Is.EqualTo("value is empty"));
    }
}

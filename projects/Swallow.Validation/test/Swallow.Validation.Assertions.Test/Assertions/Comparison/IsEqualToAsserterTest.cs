#nullable enable
using NUnit.Framework;
using Swallow.Validation.Utils;

namespace Swallow.Validation.Assertions.Comparison;

[TestFixture]
public sealed class IsEqualToAsserterTest
{
    private static readonly IsEqualToAsserter<int> Asserter = new(1);

    [Test]
    public void ReportsSuccess_WhenValueIsEqual()
    {
        var result = AssertionTester.Assert(1, Asserter, out _);

        Assert.That(result, Is.True);
    }

    [Test]
    public void ReportsError_WhenValueIsNotEqual()
    {
        var result = AssertionTester.Assert(2, Asserter, out _);

        Assert.That(result, Is.False);
    }

    [Test]
    public void ReportedError_HasCorrectMessage()
    {
        AssertionTester.Assert(2, Asserter, out var error);

        var typedError = error as NotEqualTo<int>;
        Assert.That(typedError?.ExpectedValue, Is.EqualTo(1));
        Assert.That(typedError?.Message, Is.EqualTo("value is not equal to the expected value"));
    }
}

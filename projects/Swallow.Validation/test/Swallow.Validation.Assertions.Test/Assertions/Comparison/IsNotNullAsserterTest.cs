#nullable enable
using NUnit.Framework;
using Swallow.Validation.Utils;

namespace Swallow.Validation.Assertions.Text;

[TestFixture]
public sealed class IsNotNullAsserterTest
{
    private static readonly IsNotNullAsserter<int?> Asserter = new();

    [Test]
    public void ReportsSuccess_WhenValueIsNotNull()
    {
        var result = AssertionTester.Assert(5, Asserter, out _);

        Assert.That(result, Is.True);
    }

    [Test]
    public void ReportsError_WhenValueIsNull()
    {
        var result = AssertionTester.Assert(null, Asserter, out _);

        Assert.That(result, Is.False);
    }

    [Test]
    public void ReportedError_HasCorrectMessage()
    {
        AssertionTester.Assert(null, Asserter, out var error);

        var typedError = error as ValueIsNull;
        Assert.That(typedError?.Message, Is.EqualTo("value is null"));
    }
}

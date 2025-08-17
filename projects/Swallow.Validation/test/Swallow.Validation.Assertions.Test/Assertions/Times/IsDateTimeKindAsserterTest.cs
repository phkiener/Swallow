#nullable enable
using NUnit.Framework;
using Swallow.Validation.Assertions.Times;
using Swallow.Validation.Utils;

namespace Swallow.Validation.Assertions.Text;

[TestFixture]
public sealed class IsDateTimeKindAsserterTest
{
    private static readonly IsDateTimeKindAsserter Asserter = new(DateTimeKind.Utc);

    [Test]
    public void ReportsSuccess_WhenValueHasCorrectKind()
    {
        var result = AssertionTester.Assert(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), Asserter, out _);

        Assert.That(result, Is.True);
    }

    [Test]
    public void ReportsError_WhenValueHasWrongKind()
    {
        var result = AssertionTester.Assert(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local), Asserter, out _);

        Assert.That(result, Is.False);
    }

    [Test]
    public void ReportedError_HasCorrectMessage()
    {
        AssertionTester.Assert(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local), Asserter, out var error);

        var typedError = error as WrongDateTimeKind;
        Assert.That(typedError?.Message, Is.EqualTo("value is not of kind Utc"));
    }
}

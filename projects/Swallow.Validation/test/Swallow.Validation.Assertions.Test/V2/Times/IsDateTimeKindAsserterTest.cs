#nullable enable
using NUnit.Framework;

namespace Swallow.Validation.V2.Times;

[TestFixture]
public sealed class IsDateTimeKindAsserterTest
{
    private static readonly IsDateTimeKindAsserter Asserter = new(DateTimeKind.Utc);

    [Test]
    public void ReportsSuccess_WhenValueHasCorrectKind()
    {
        Assert.That(Asserter.IsValid(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)), Is.True);
    }

    [Test]
    public void ReportsError_WhenValueHasWrongKind()
    {
        Assert.That(Asserter.IsValid(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)), Is.False);
    }

    [Test]
    public void ReturnsExpectedError()
    {
        var typedError = Asserter.Error as WrongDateTimeKind;
        Assert.That(typedError?.Message, Is.EqualTo("Value is not of kind Utc"));
    }
}

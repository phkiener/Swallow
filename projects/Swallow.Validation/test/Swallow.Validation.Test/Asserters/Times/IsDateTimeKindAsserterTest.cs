using NUnit.Framework;

namespace Swallow.Validation.Next.Asserters.Times;

[TestFixture]
public sealed class IsDateTimeKindAsserterTest
{
    [Test]
    public void ReportsSuccess_WhenValueHasCorrectKind()
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        Assert.That(Satisfies.HasKind(DateTimeKind.Utc).IsValid(dateTime), Is.True);
    }

    [Test]
    public void ReportsError_WhenValueHasWrongKind()
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
        Assert.That(Satisfies.HasKind(DateTimeKind.Utc).IsValid(dateTime), Is.False);
    }

    [Test]
    public void ReturnsExpectedError()
    {
        var typedError = Satisfies.HasKind(DateTimeKind.Utc).Error as WrongDateTimeKind;
        Assert.That(typedError?.Message, Is.EqualTo("have kind Utc"));
    }
}

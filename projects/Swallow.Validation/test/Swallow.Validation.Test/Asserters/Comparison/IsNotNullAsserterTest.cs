using NUnit.Framework;

namespace Swallow.Validation.Next.Asserters.Comparison;

[TestFixture]
public sealed class IsNotNullAsserterTest
{
    [Test]
    public void ReportsSuccess_WhenValueIsNotNull()
    {
        Assert.That(Satisfies.NotNull.IsValid(1), Is.True);
        Assert.That(Satisfies.Null.IsValid(1), Is.False);
    }

    [Test]
    public void ReportsError_WhenValueIsNull()
    {
        Assert.That(Satisfies.NotNull.IsValid(null), Is.False);
        Assert.That(Satisfies.Null.IsValid(1), Is.False);
    }

    [Test]
    public void ReturnsExpectedError()
    {
        var typedError = Satisfies.NotNull.Error as ValueIsNull;
        Assert.That(typedError?.Message, Is.EqualTo("be not null"));

        var invertedError = Satisfies.Null.Error;
        Assert.That(invertedError.Message, Is.EqualTo("not be not null"));
    }
}

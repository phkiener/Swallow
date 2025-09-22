using NUnit.Framework;

namespace Swallow.Validation.Next.Asserters.Comparison;

[TestFixture]
public sealed class IsEqualToAsserterTest
{
    [Test]
    public void ReportsSuccess_WhenValueIsEqual()
    {
        Assert.That(Satisfies.EqualTo(1).IsValid(1), Is.True);
    }

    [Test]
    public void ReportsError_WhenValueIsNotEqual()
    {
        Assert.That(Satisfies.EqualTo(1).IsValid(2), Is.False);
    }

    [Test]
    public void ReturnsExpectedError()
    {
        var typedError = Satisfies.EqualTo(1).Error as NotEqualTo<int>;
        Assert.That(typedError?.ExpectedValue, Is.EqualTo(1));
        Assert.That(typedError?.Message, Is.EqualTo("be 1"));
    }
}

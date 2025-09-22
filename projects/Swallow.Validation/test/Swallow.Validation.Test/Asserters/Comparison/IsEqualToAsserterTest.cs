using NUnit.Framework;

namespace Swallow.Validation.Next.Asserters.Comparison;

[TestFixture]
public sealed class IsEqualToAsserterTest
{
    private static readonly IsEqualToAsserter<int> Asserter = new(1);

    [Test]
    public void ReportsSuccess_WhenValueIsEqual()
    {
        Assert.That(Asserter.IsValid(1), Is.True);
    }

    [Test]
    public void ReportsError_WhenValueIsNotEqual()
    {
        Assert.That(Asserter.IsValid(2), Is.False);
    }

    [Test]
    public void ReturnsExpectedError()
    {
        var typedError = Asserter.Error as NotEqualTo<int>;
        Assert.That(typedError?.ExpectedValue, Is.EqualTo(1));
        Assert.That(typedError?.Message, Is.EqualTo("be 1"));
    }
}

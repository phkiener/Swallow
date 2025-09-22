using NUnit.Framework;

namespace Swallow.Validation.Next.Asserters.Text;

[TestFixture]
public sealed class IsNotEmptyAsserterTest
{
    private static readonly IsNotEmptyAsserter Asserter = new();

    [Test]
    public void ReportsSuccess_WhenValueIsNotEmpty()
    {
        Assert.That(Asserter.IsValid("hello"), Is.True);
    }

    [Test]
    public void ReportsError_WhenValueIsEmpty()
    {
        Assert.That(Asserter.IsValid(""), Is.False);
    }

    [Test]
    public void ReturnsExpectedError()
    {
        var typedError = Asserter.Error as EmptyString;
        Assert.That(typedError?.Message, Is.EqualTo("be not empty"));
    }
}

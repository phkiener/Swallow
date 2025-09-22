using NUnit.Framework;

namespace Swallow.Validation.Next.Asserters.Text;

[TestFixture]
public sealed class IsNotEmptyAsserterTest
{
    [Test]
    public void ReportsSuccess_WhenValueIsNotEmpty()
    {
        Assert.That(Satisfies.NotEmpty.IsValid("hello"), Is.True);
    }

    [Test]
    public void ReportsError_WhenValueIsEmpty()
    {
        Assert.That(Satisfies.NotEmpty.IsValid(""), Is.False);
    }

    [Test]
    public void ReturnsExpectedError()
    {
        var typedError = Satisfies.NotEmpty.Error as EmptyString;
        Assert.That(typedError?.Message, Is.EqualTo("be not empty"));
    }
}

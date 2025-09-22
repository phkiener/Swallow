using NUnit.Framework;

namespace Swallow.Validation.Next.Asserters.Text;

[TestFixture]
public sealed class IsNotEmptyOrWhitespaceAsserterTest
{
    [Test]
    public void ReportsSuccess_WhenValueIsNotEmpty()
    {
        Assert.That(Satisfies.NotEmptyOrWhitespace.IsValid("hello"), Is.True);
    }

    [Test]
    public void ReportsError_WhenValueIsEmpty()
    {
        Assert.That(Satisfies.NotEmptyOrWhitespace.IsValid(""), Is.False);
        Assert.That(Satisfies.NotEmptyOrWhitespace.IsValid("   "), Is.False);
        Assert.That(Satisfies.NotEmptyOrWhitespace.IsValid("\n\r\t"), Is.False);
    }

    [Test]
    public void ReportedError_HasCorrectMessage()
    {
        var typedError = Satisfies.NotEmptyOrWhitespace.Error as OnlyWhitespaceString;
        Assert.That(typedError?.Message, Is.EqualTo("be not only whitespace"));
    }
}

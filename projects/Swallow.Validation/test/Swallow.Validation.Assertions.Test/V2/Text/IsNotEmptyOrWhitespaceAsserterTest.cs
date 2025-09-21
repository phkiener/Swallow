#nullable enable
using NUnit.Framework;

namespace Swallow.Validation.V2.Text;

[TestFixture]
public sealed class IsNotEmptyOrWhitespaceAsserterTest
{
    private static readonly IsNotEmptyOrWhitespaceAsserter Asserter = new();

    [Test]
    public void ReportsSuccess_WhenValueIsNotEmpty()
    {
        Assert.That(Asserter.IsValid("hello"), Is.True);
    }

    [Test]
    public void ReportsError_WhenValueIsEmpty()
    {
        Assert.That(Asserter.IsValid(""), Is.False);
        Assert.That(Asserter.IsValid("   "), Is.False);
        Assert.That(Asserter.IsValid("\n\r\t"), Is.False);
    }

    [Test]
    public void ReportedError_HasCorrectMessage()
    {
        var typedError = Asserter.Error as OnlyWhitespaceString;
        Assert.That(typedError?.Message, Is.EqualTo("value contains only whitespace characters"));
    }
}

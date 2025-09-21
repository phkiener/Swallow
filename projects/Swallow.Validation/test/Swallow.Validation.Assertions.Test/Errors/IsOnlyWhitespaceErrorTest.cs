namespace Swallow.Validation.Errors;

using NUnit.Framework;

[TestFixture]
public sealed class IsOnlyWhitespaceErrorTest
{
    [Test]
    public void HaveCorrectMessage()
    {
        var error = new IsOnlyWhitespaceError { PropertyName = "SomeText" };
        Assert.That(error.Message, Is.EqualTo("SomeText must contain at least one non-whitespace character."));
    }
}

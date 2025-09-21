namespace Swallow.Validation.Errors;

using NUnit.Framework;

[TestFixture]
public sealed class IsNullValidationErrorTest
{
    [Test]
    public void HaveMessage()
    {
        var error = new IsNullValidationError { PropertyName = "nullable" };
        Assert.That(error.Message, Is.EqualTo("nullable must not be null"));
    }
}

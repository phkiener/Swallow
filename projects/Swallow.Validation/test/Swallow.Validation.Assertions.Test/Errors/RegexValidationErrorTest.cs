namespace Swallow.Validation.Errors;

using NUnit.Framework;

[TestFixture]
public sealed class RegexValidationErrorTest
{
    [Test]
    public void HaveMessageWithDescriptionAndRegex()
    {
        var error = new RegexValidationError(testedRegex: "[0-9]+", targetDescription: "number") { PropertyName = "string", ActualValue = "hello" };
        Assert.That(error.Message, Is.EqualTo("string must be a valid number but was 'hello'"));
    }

    [Test]
    public void HaveMessageWithoutDescriptionOrRegex()
    {
        var error = new RegexValidationError(null) { PropertyName = "string", ActualValue = "hello" };
        Assert.That(error.Message, Is.EqualTo("string did not match the expected format"));
    }

    [Test]
    public void HaveMessageWithRegex()
    {
        var error = new RegexValidationError("[0-9]+") { PropertyName = "string", ActualValue = "hello" };
        Assert.That(error.Message, Is.EqualTo("string must match '[0-9]+' but was 'hello'"));
    }
}

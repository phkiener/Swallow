namespace Swallow.Validation.Errors;

using NUnit.Framework;

[TestFixture]
public sealed class EntityValidationErrorTest
{
    [Test]
    public void HaveMessageWithInnerErrors()
    {
        var error = new EntityValidationError(
        [
            new GenericValidationError("be valid") { PropertyName = "Property" },
            new GenericValidationError("be valid too") { PropertyName = "OtherProperty" }
        ]) { PropertyName = "MyObject" };

        Assert.That(error.Message, Is.EqualTo("""
                                              MyObject failed validation
                                                (1/2) Property must be valid but was 'null'
                                                (2/2) OtherProperty must be valid too but was 'null'
                                              """));
    }

    [Test]
    public void HaveMessageWithInnerErrorsWithinInnerErrors()
    {
        var innermostError = new IsNullValidationError { PropertyName = "Value" };
        var innerErrors = new[] { new EntityValidationError([innermostError]) { PropertyName = "Object" } };
        var error = new EntityValidationError(innerErrors) { PropertyName = "Collection" };

        Assert.That(error.Message, Is.EqualTo("""
                                              Collection failed validation
                                                (1/1) Object failed validation
                                                  (1/1) Value must not be null
                                              """));
    }

    [Test]
    public void HaveMessageWithoutInnerErrors()
    {
        var error = new EntityValidationError([]) { PropertyName = "MyObject" };
        Assert.That(error.Message, Is.EqualTo("MyObject failed validation"));
    }
}

namespace Swallow.Validation.Errors;

using NUnit.Framework;

[TestFixture]
public sealed class EmptyCollectionValidationErrorTest
{
    [Test]
    public void HaveMessage()
    {
        var error = new EmptyCollectionValidationError { PropertyName = "Collection" };

        Assert.That(error.Message, Is.EqualTo("Collection must not be empty"));
    }
}

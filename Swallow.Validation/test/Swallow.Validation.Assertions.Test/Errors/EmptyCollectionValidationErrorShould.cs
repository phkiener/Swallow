namespace Swallow.Validation.Errors;

using FluentAssertions;
using NUnit.Framework;

[TestFixture]
internal sealed class EmptyCollectionValidationErrorShould
{
    [Test]
    public void HaveMessage()
    {
        // Arrange
        var error = new EmptyCollectionValidationError { PropertyName = "Collection" };

        // Act
        var result = error.Message;

        // Assert
        result.Should().Be("Collection must not be empty");
    }
}

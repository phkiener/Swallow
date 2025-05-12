namespace Swallow.Validation.Errors;

using FluentAssertions;
using NUnit.Framework;

[TestFixture]
internal sealed class IsNullValidationErrorShould
{
    [Test]
    public void HaveMessage()
    {
        // Arrange
        var error = new IsNullValidationError { PropertyName = "nullable" };

        // Act
        var result = error.Message;

        // Assert
        result.Should().Be("nullable must not be null");
    }
}

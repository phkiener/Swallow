namespace Swallow.Validation.Errors;

using FluentAssertions;
using NUnit.Framework;

[TestFixture]
internal sealed class IsOnlyWhitespaceErrorShould
{
    [Test]
    public void HaveCorrectMessage()
    {
        // Arrange
        var error = new IsOnlyWhitespaceError { PropertyName = "SomeText" };

        // Act
        var message = error.Message;

        // Assert
        message.Should().Be("SomeText must contain at least one non-whitespace character.");
    }
}

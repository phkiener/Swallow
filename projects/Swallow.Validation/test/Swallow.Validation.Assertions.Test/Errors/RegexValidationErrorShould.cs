namespace Swallow.Validation.Errors;

using FluentAssertions;
using NUnit.Framework;

[TestFixture]
internal sealed class RegexValidationErrorShould
{
    [Test]
    public void HaveMessageWithDescriptionAndRegex()
    {
        // Arrange
        var error = new RegexValidationError(testedRegex: "[0-9]+", targetDescription: "number") { PropertyName = "string", ActualValue = "hello" };

        // Act
        var result = error.Message;

        // Assert
        result.Should().Be("string must be a valid number but was 'hello'");
    }

    [Test]
    public void HaveMessageWithoutDescriptionOrRegex()
    {
        // Arrange
        var error = new RegexValidationError(null) { PropertyName = "string", ActualValue = "hello" };

        // Act
        var result = error.Message;

        // Assert
        result.Should().Be("string did not match the expected format");
    }

    [Test]
    public void HaveMessageWithRegex()
    {
        // Arrange
        var error = new RegexValidationError("[0-9]+") { PropertyName = "string", ActualValue = "hello" };

        // Act
        var result = error.Message;

        // Assert
        result.Should().Be("string must match '[0-9]+' but was 'hello'");
    }
}

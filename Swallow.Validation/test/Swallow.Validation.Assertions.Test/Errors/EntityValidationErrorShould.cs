namespace Swallow.Validation.Errors;

using System;
using FluentAssertions;
using NUnit.Framework;

[TestFixture]
internal sealed class EntityValidationErrorShould
{
    [Test]
    public void HaveMessageWithInnerErrors()
    {
        // Arrange
        var error = new EntityValidationError(
            new[]
            {
                new GenericValidationError("be valid") { PropertyName = "Property" },
                new GenericValidationError("be valid too") { PropertyName = "OtherProperty" }
            }) { PropertyName = "MyObject" };

        // Act
        var result = error.Message;

        // Assert
        var expected = string.Join(
            separator: Environment.NewLine,
            "MyObject failed validation",
            "  (1/2) Property must be valid but was 'null'",
            "  (2/2) OtherProperty must be valid too but was 'null'");

        result.Should().Be(expected);
    }

    [Test]
    public void HaveMessageWithInnerErrorsWithinInnerErrors()
    {
        // Arrange
        var innermostError = new IsNullValidationError { PropertyName = "Value" };
        var innerErrors = new[] { new EntityValidationError(new[] { innermostError }) { PropertyName = "Object" } };
        var error = new EntityValidationError(innerErrors) { PropertyName = "Collection" };

        // Act
        var result = error.Message;

        // Assert
        var expectedMessage = string.Join(
            separator: Environment.NewLine,
            "Collection failed validation",
            "  (1/1) Object failed validation",
            "    (1/1) Value must not be null");

        result.Should().Be(expectedMessage);
    }

    [Test]
    public void HaveMessageWithoutInnerErrors()
    {
        // Arrange
        var error = new EntityValidationError(Array.Empty<ValidationError>()) { PropertyName = "MyObject" };

        // Act
        var result = error.Message;

        // Assert
        result.Should().Be("MyObject failed validation");
    }
}

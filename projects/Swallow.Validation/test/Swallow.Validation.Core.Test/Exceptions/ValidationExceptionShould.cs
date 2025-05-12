namespace Swallow.Validation.Exceptions;

using System;
using System.Linq;
using Errors;
using FluentAssertions;
using NUnit.Framework;
using TestUtils;

[TestFixture]
internal sealed class ValidationExceptionShould
{
    [Test]
    public void SetMessageWithoutErrors()
    {
        // Act
        var result = new ValidationException(Enumerable.Empty<ValidationError>());

        // Assert
        result.Message.Should().Be("Validation failed");
    }

    [Test]
    public void SetMessageWithSingleError()
    {
        // Act
        var result = new ValidationException(new[] { new TestValidationError() });

        // Assert
        var expectedMessage = string.Join(separator: Environment.NewLine, "Validation failed", "(1/1) An error");
        result.Message.Should().Be(expectedMessage);
    }

    [Test]
    public void SetMessageWithMultipleErrors()
    {
        // Act
        var result = new ValidationException(new[] { new TestValidationError(), new TestValidationError() });

        // Assert
        var expectedMessage = string.Join(separator: Environment.NewLine, "Validation failed", "(1/2) An error", "(2/2) An error");
        result.Message.Should().Be(expectedMessage);
    }
}

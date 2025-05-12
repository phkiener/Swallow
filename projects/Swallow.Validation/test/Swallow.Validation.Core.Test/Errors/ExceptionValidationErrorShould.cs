namespace Swallow.Validation.Errors;

using System;
using FluentAssertions;
using NUnit.Framework;

[TestFixture]
public class ExceptionValidationErrorShould
{
    [Test]
    public void SetMessageToMessageOfInnerException()
    {
        // Act
        var exception = new InvalidOperationException("Something happened!");
        var error = new ExceptionValidationError(exception);

        // Assert
        error.Message.Should().Be($"{exception.GetType().FullName}: {exception.Message}");
    }
}

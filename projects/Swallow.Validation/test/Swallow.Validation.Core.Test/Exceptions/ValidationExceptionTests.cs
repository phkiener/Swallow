namespace Swallow.Validation.Exceptions;

using System;
using NUnit.Framework;
using TestUtils;

[TestFixture]
public sealed class ValidationExceptionTests
{
    [Test]
    public void SetMessageWithoutErrors()
    {
        var result = new ValidationException([]);

        Assert.That(result.Message, Is.EqualTo("Validation failed"));
    }

    [Test]
    public void SetMessageWithSingleError()
    {
        var result = new ValidationException([new TestValidationError()]);

        var expectedMessage = string.Join(separator: Environment.NewLine, "Validation failed", "(1/1) An error");
        Assert.That(result.Message, Is.EqualTo(expectedMessage));
    }

    [Test]
    public void SetMessageWithMultipleErrors()
    {
        var result = new ValidationException([new TestValidationError(), new TestValidationError()]);

        var expectedMessage = string.Join(separator: Environment.NewLine, "Validation failed", "(1/2) An error", "(2/2) An error");
        Assert.That(result.Message, Is.EqualTo(expectedMessage));
    }
}

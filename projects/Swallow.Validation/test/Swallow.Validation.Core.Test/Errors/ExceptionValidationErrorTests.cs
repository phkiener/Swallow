namespace Swallow.Validation.Errors;

using System;
using NUnit.Framework;

[TestFixture]
public sealed class ExceptionValidationErrorTests
{
    [Test]
    public void SetMessageToMessageOfInnerException()
    {
        var exception = new InvalidOperationException("Something happened!");
        var error = new ExceptionValidationError(exception);

        Assert.That(error.Message, Is.EqualTo($"{exception.GetType().FullName}: {exception.Message}"));
    }
}

namespace Swallow.Validation.Errors;

using System.Collections.Generic;
using NUnit.Framework;

[TestFixture]
public sealed class DisallowedValueValidationErrorTest
{
    [Test]
    public void HaveMessageForValueBeingInSet()
    {
        var error = DisallowedValueValidationError<int>.BeOneOf(new List<int> { 2, 3, 5, 7, 9 });
        error.PropertyName = "Value";
        error.ActualValue = "1";

        Assert.That(error.Message, Is.EqualTo("Value must be in (2, 3, 5, 7, 9) but was 1"));
    }

    [Test]
    public void HaveMessageForValueNotBeingInSet()
    {
        var error = DisallowedValueValidationError<int>.NotBeOneOf(new List<int> { 2, 3, 5, 7, 9 });
        error.PropertyName = "Value";
        error.ActualValue = "2";

        Assert.That(error.Message, Is.EqualTo("Value may not be in (2, 3, 5, 7, 9) but was 2"));
    }
}

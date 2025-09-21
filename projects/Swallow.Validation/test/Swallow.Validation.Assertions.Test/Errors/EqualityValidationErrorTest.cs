namespace Swallow.Validation.Errors;

using NUnit.Framework;

[TestFixture]
public sealed class EqualityValidationErrorTest
{
    [Test]
    public void HaveMessageForEqualValues()
    {
        var error = EqualityValidationError<int>.MustBe(12);
        error.PropertyName = "value";
        error.ActualValue = "11";

        Assert.That(error.Message, Is.EqualTo("value should be 12 but was 11"));
    }

    [Test]
    public void HaveMessageForNotEqualValues()
    {
        var error = EqualityValidationError<int>.MustNotBe(12);
        error.PropertyName = "value";
        error.ActualValue = "12";

        Assert.That(error.Message, Is.EqualTo("value should not be 12"));
    }
}

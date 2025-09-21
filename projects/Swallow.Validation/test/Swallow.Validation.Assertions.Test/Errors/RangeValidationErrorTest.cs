namespace Swallow.Validation.Errors;

using NUnit.Framework;

[TestFixture]
public sealed class RangeValidationErrorTest
{
    [Test]
    public void HaveMessageForBothBoundsSet()
    {
        var error = new RangeValidationError<int>(lowerBound: 10, upperBound: 100) { PropertyName = "value", ActualValue = "1000" };
        Assert.That(error.Message, Is.EqualTo("value must be between 10 and 100, but was 1000"));
    }

    [Test]
    public void HaveMessageForLowerInclusiveBound()
    {
        var error = RangeValidationError<int>.FromLowerBound(value: 10, isInclusive: false);
        error.PropertyName = "value";
        error.ActualValue = "1";

        Assert.That(error.Message, Is.EqualTo("value must be greater than 10, but was 1"));
    }

    [Test]
    public void HaveMessageForLowerBound()
    {
        var error = RangeValidationError<int>.FromLowerBound(value: 10, isInclusive: true);
        error.PropertyName = "value";
        error.ActualValue = "1";

        Assert.That(error.Message, Is.EqualTo("value must be greater than or equal to 10, but was 1"));
    }

    [Test]
    public void HaveMessageForUpperBound()
    {
        var error = RangeValidationError<int>.FromUpperBound(value: 10, isInclusive: false);
        error.PropertyName = "value";
        error.ActualValue = "1000";

        Assert.That(error.Message, Is.EqualTo("value must be less than 10, but was 1000"));
    }

    [Test]
    public void HaveMessageForUpperInclusiveBound()
    {
        var error = RangeValidationError<int>.FromUpperBound(value: 10, isInclusive: true);
        error.PropertyName = "value";
        error.ActualValue = "1000";

        Assert.That(error.Message, Is.EqualTo("value must be less than or equal to 10, but was 1000"));
    }
}

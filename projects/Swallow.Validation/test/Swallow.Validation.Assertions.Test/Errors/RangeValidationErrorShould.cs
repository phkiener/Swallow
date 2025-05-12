namespace Swallow.Validation.Errors;

using FluentAssertions;
using NUnit.Framework;

[TestFixture]
internal sealed class RangeValidationErrorShould
{
    [Test]
    public void HaveMessageForBothBoundsSet()
    {
        // Arrange
        var error = new RangeValidationError<int>(lowerBound: 10, upperBound: 100) { PropertyName = "value", ActualValue = "1000" };

        // Act
        var result = error.Message;

        // Assert
        result.Should().Be("value must be between 10 and 100, but was 1000");
    }

    [Test]
    public void HaveMessageForLowerInclusiveBound()
    {
        // Arrange
        var error = RangeValidationError<int>.FromLowerBound(value: 10, isInclusive: false);
        error.PropertyName = "value";
        error.ActualValue = "1";

        // Act
        var result = error.Message;

        // Assert
        result.Should().Be("value must be greater than 10, but was 1");
    }

    [Test]
    public void HaveMessageForLowerBound()
    {
        // Arrange
        var error = RangeValidationError<int>.FromLowerBound(value: 10, isInclusive: true);
        error.PropertyName = "value";
        error.ActualValue = "1";

        // Act
        var result = error.Message;

        // Assert
        result.Should().Be("value must be greater than or equal to 10, but was 1");
    }

    [Test]
    public void HaveMessageForUpperBound()
    {
        // Arrange
        var error = RangeValidationError<int>.FromUpperBound(value: 10, isInclusive: false);
        error.PropertyName = "value";
        error.ActualValue = "1000";

        // Act
        var result = error.Message;

        // Assert
        result.Should().Be("value must be less than 10, but was 1000");
    }

    [Test]
    public void HaveMessageForUpperInclusiveBound()
    {
        // Arrange
        var error = RangeValidationError<int>.FromUpperBound(value: 10, isInclusive: true);
        error.PropertyName = "value";
        error.ActualValue = "1000";

        // Act
        var result = error.Message;

        // Assert
        result.Should().Be("value must be less than or equal to 10, but was 1000");
    }
}

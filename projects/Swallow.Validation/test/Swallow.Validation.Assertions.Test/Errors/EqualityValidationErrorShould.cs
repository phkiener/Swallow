namespace Swallow.Validation.Errors;

using FluentAssertions;
using NUnit.Framework;

[TestFixture]
internal sealed class EqualityValidationErrorShould
{
    [Test]
    public void HaveMessageForEqualValues()
    {
        // Arrange
        var error = EqualityValidationError<int>.MustBe(12);
        error.PropertyName = "value";
        error.ActualValue = "11";

        // Act
        var result = error.Message;

        // Assert
        result.Should().Be("value should be 12 but was 11");
    }

    [Test]
    public void HaveMessageForNotEqualValues()
    {
        // Arrange
        var error = EqualityValidationError<int>.MustNotBe(12);
        error.PropertyName = "value";
        error.ActualValue = "12";

        // Act
        var result = error.Message;

        // Assert
        result.Should().Be("value should not be 12");
    }
}

namespace Swallow.Validation.Errors;

using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

[TestFixture]
internal sealed class DisallowedValueValidationErrorShould
{
    [Test]
    public void HaveMessageForValueBeingInSet()
    {
        // Arrange
        var error = DisallowedValueValidationError<int>.BeOneOf(
            new List<int>
            {
                2,
                3,
                5,
                7,
                9
            });

        error.PropertyName = "Value";
        error.ActualValue = "1";

        // Act
        var result = error.Message;

        // Assert
        result.Should().Be("Value must be in (2, 3, 5, 7, 9) but was 1");
    }

    [Test]
    public void HaveMessageForValueNotBeingInSet()
    {
        // Arrange
        var error = DisallowedValueValidationError<int>.NotBeOneOf(
            new List<int>
            {
                2,
                3,
                5,
                7,
                9
            });

        error.PropertyName = "Value";
        error.ActualValue = "2";

        // Act
        var result = error.Message;

        // Assert
        result.Should().Be("Value may not be in (2, 3, 5, 7, 9) but was 2");
    }
}

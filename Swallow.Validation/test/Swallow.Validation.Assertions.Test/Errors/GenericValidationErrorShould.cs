namespace Swallow.Validation.Errors;

using FluentAssertions;
using NUnit.Framework;

[TestFixture]
internal sealed class GenericValidationErrorShould
{
    [Test]
    [TestCase(arg1: null, arg2: "value must satisfy predicate but was 'violation'")]
    [TestCase(arg1: "satisfy condition", arg2: "value must satisfy condition but was 'violation'")]
    public void HaveMessage(string state, string expected)
    {
        // Arrange
        var error = new GenericValidationError(state) { PropertyName = "value", ActualValue = "violation" };

        // Act
        var result = error.Message;

        // Assert
        result.Should().Be(expected);
    }
}

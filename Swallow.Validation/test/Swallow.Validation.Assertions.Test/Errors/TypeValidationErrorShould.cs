namespace Swallow.Validation.Errors;

using FluentAssertions;
using NUnit.Framework;

[TestFixture]
internal sealed class TypeValidationErrorShould
{
    [Test]
    public void HaveMessageForTypeBeingEqual()
    {
        // Arrange
        var error = new TypeValidationError(expectedType: typeof(int), actualType: typeof(string), mode: TypeValidationError.MatchingMode.SameType)
        {
            PropertyName = "Property", ActualValue = "teehee"
        };

        // Act
        var message = error.Message;

        // Assert
        message.Should().Be("Property should be of type System.Int32 but was System.String");
    }

    [Test]
    public void HaveMessageForTypeBeingAssignableTo()
    {
        // Arrange
        var error = new TypeValidationError(
            expectedType: typeof(int),
            actualType: typeof(string),
            mode: TypeValidationError.MatchingMode.AssignableTo) { PropertyName = "Property", ActualValue = "teehee" };

        // Act
        var message = error.Message;

        // Assert
        message.Should().Be("Property should be assignable to System.Int32 but was System.String, which is not");
    }
}

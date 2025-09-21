namespace Swallow.Validation.Errors;

using NUnit.Framework;

[TestFixture]
public sealed class TypeValidationErrorTest
{
    [Test]
    public void HaveMessageForTypeBeingEqual()
    {
        var error = new TypeValidationError(
            expectedType: typeof(int),
            actualType: typeof(string),
            mode: TypeValidationError.MatchingMode.SameType) { PropertyName = "Property", ActualValue = "teehee" };

        Assert.That(error.Message, Is.EqualTo("Property should be of type System.Int32 but was System.String"));
    }

    [Test]
    public void HaveMessageForTypeBeingAssignableTo()
    {
        var error = new TypeValidationError(
            expectedType: typeof(int),
            actualType: typeof(string),
            mode: TypeValidationError.MatchingMode.AssignableTo) { PropertyName = "Property", ActualValue = "teehee" };

        Assert.That(error.Message, Is.EqualTo("Property should be assignable to System.Int32 but was System.String, which is not"));
    }
}

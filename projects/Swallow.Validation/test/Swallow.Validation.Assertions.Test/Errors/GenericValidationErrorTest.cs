namespace Swallow.Validation.Errors;

using NUnit.Framework;

[TestFixture]
public sealed class GenericValidationErrorTest
{
    [Test]
    [TestCase(arg1: null, arg2: "value must satisfy predicate but was 'violation'")]
    [TestCase(arg1: "satisfy condition", arg2: "value must satisfy condition but was 'violation'")]
    public void HaveMessage(string state, string expected)
    {
        var error = new GenericValidationError(state) { PropertyName = "value", ActualValue = "violation" };
        Assert.That(error.Message, Is.EqualTo(expected));
    }
}

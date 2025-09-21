namespace Swallow.Validation.Assertions;

using Errors;
using NUnit.Framework;
using Utils;

[TestFixture]
public sealed class ValidatableEntityExtensionsTest
{
    [Test]
    public void FailAssertion_WhenValidatableEntityIsNotValid()
    {
        var value = new TestValidatableEntity { ShouldSucceed = false };
        var result = AssertionTester.Assert(value: value, assertion: v => v.IsValid());

        Assert.That(result.Errors, Has.One.InstanceOf<EntityValidationError>());
    }

    [Test]
    public void PassAssertion_WhenValidatableEntityIsValid()
    {
        var value = new TestValidatableEntity { ShouldSucceed = true };
        var result = AssertionTester.Assert(value: value, assertion: v => v.IsValid());

        Assert.That(result.IsSuccess, Is.True);
    }
}

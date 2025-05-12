namespace Swallow.Validation.Assertions;

using Errors;
using NUnit.Framework;
using Utils;

[TestFixture]
internal sealed class ValidatableEntityExtensionsShould
{
    [Test]
    public void FailAssertion_WhenValidatableEntityIsNotValid()
    {
        // Act
        var value = new TestValidatableEntity { ShouldSucceed = false };
        var result = AssertionTester.Assert(value: value, assertion: v => v.IsValid());

        // Assert
        result.Should().HaveError<EntityValidationError>();
    }

    [Test]
    public void PassAssertion_WhenValidatableEntityIsValid()
    {
        // Act
        var value = new TestValidatableEntity { ShouldSucceed = true };
        var result = AssertionTester.Assert(value: value, assertion: v => v.IsValid());

        // Assert
        result.Should().BeSuccess();
    }
}

namespace Swallow.Validation.Validatable;

using Errors;
using FluentAssertions;
using NUnit.Framework;
using TestUtils;
using Utils;

[TestFixture]
internal class ValidatableEntityAsserterShould
{
    public sealed class OnCheck : ValidatableEntityAsserterShould
    {
        [Test]
        public void ReturnFalse_WhenCheckedValueIsNotValid()
        {
            // Arrange
            var entity = new TestValidatableEntity { ShouldSucceed = false };
            var asserter = new ValidatableEntityAsserter<TestValidatableEntity>();
            var valueProvider = TestValue.Of(entity);

            // Act
            var result = asserter.Check(valueProvider: valueProvider, error: out var error);

            // Assert
            result.Should().BeFalse();
            error.Should().BeOfType<EntityValidationError>().Which.ValidationErrors.Should().HaveCount(1);
        }

        [Test]
        public void ReturnTrue_WhenCheckedValueIsValid()
        {
            // Arrange
            var entity = new TestValidatableEntity { ShouldSucceed = true };
            var asserter = new ValidatableEntityAsserter<TestValidatableEntity>();
            var valueProvider = TestValue.Of(entity);

            // Act
            var result = asserter.Check(valueProvider: valueProvider, error: out var error);

            // Assert
            result.Should().BeTrue();
            error.Should().BeNull();
        }
    }
}

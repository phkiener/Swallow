namespace Swallow.Validation;

using Errors;
using FluentAssertions;
using NUnit.Framework;
using TestUtils;

[TestFixture]
internal class SimpleAsserterTest
{
    private sealed class DerivedSimpleAsserter : SimpleAsserter<int>
    {
        public bool ShouldSucceed { get; init; } = true;

        protected override ValidationResult Validate(int value)
        {
            return Validator.Check()
                .That(value: value, name: PropertyName)
                .Satisfies(predicate: _ => ShouldSucceed, errorFunc: _ => new TestValidationError())
                .Result();
        }
    }

    public sealed class OnCheck : SimpleAsserterTest
    {
        [Test]
        public void ReturnFalse_WhenAsserterFails()
        {
            // Arrange
            var valueProvider = TestValue.Of(1337);
            var asserter = new DerivedSimpleAsserter { ShouldSucceed = false };

            // Act
            var result = asserter.Check(valueProvider: valueProvider, error: out var error);

            // Assert
            result.Should().BeFalse();
            error.Should()
                .NotBeNull()
                .And.BeOfType<EntityValidationError>()
                .Which.ValidationErrors.Should()
                .OnlyContain(e => e.PropertyName == valueProvider.Name);
        }

        [Test]
        public void Check_ValidateProducesSuccess_ReturnsTrueAndSetsErrorToNull()
        {
            // Arrange
            var valueProvider = TestValue.Of(42);
            var asserter = new DerivedSimpleAsserter { ShouldSucceed = true };

            // Act
            var result = asserter.Check(valueProvider: valueProvider, error: out var error);

            // Assert
            result.Should().BeTrue();
            error.Should().BeNull();
        }
    }
}

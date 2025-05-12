namespace Swallow.Validation;

using System;
using System.Linq;
using Errors;
using FluentAssertions;
using NUnit.Framework;
using TestUtils;

[TestFixture]
internal class ValidatorShould
{
    public sealed class OnConfiguredBehavior : ValidatorShould
    {
        [Test]
        public void ReportExceptionsAsExceptionValidationError_WhenCatchExceptionsIsEnabled()
        {
            // Arrange
            var asserter = new ThrowingAsserter<int>();

            // Act
            var result = Validator.Check(new() { CatchExceptions = true }).That(TestValue.Of(5)).Satisfies(asserter).Result();

            // Assert
            result.Errors.Should().HaveCount(1);
            result.Errors.Single().Should().BeOfType<ExceptionValidationError>().Which.Exception.Should().Be(asserter.ThrownException);
        }

        [Test]
        public void ThrowExceptionFromAsserter_WhenCatchExceptionsIsDisabled()
        {
            // Arrange
            var asserter = new ThrowingAsserter<int>();

            // Act
            var act = () => Validator.Check(new() { CatchExceptions = false }).That(TestValue.Of(5)).Satisfies(asserter).Result();

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage(asserter.ThrownException.Message);
        }

        [Test]
        public void EvaluateAssertionAsSoonAsPossible_WhenValidateLazilyIsDisabled()
        {
            // Arrange
            var asserter = TestAsserter.Succeeding<int>();
            var followingAsserter = TestAsserter.Succeeding<int>();

            // Act
            Validator.Check(new() { ValidateLazily = false }).That(TestValue.Of(5)).Satisfies(asserter).Satisfies(followingAsserter);

            // Assert
            asserter.TimesCalled.Should().Be(1);
        }

        [Test]
        public void NotEvaluateAssertion_WhenValidateLazilyIsEnabledAndResultIsNotCalled()
        {
            // Arrange
            var asserter = TestAsserter.Succeeding<int>();
            var followingAsserter = TestAsserter.Succeeding<int>();

            // Act
            Validator.Check(new() { ValidateLazily = true }).That(TestValue.Of(5)).Satisfies(asserter).Satisfies(followingAsserter);

            // Assert
            asserter.TimesCalled.Should().Be(0);
        }

        [Test]
        public void EvaluateAssertion_WhenValidateLazilyIsEnabledAndResultIsCalled()
        {
            // Arrange
            var asserter = TestAsserter.Failing<int>();
            var followingAsserter = TestAsserter.Succeeding<int>();

            // Act
            Validator.Check(new() { ValidateLazily = true }).That(TestValue.Of(5)).Satisfies(asserter).Satisfies(followingAsserter).Result();

            // Assert
            asserter.TimesCalled.Should().Be(1);
        }

        [Test]
        public void ContinueAssertingOnErrors_WhenFailureHandlingIsKeepGoing()
        {
            // Arrange
            var failingAsserter = TestAsserter.Failing<int>();
            var succeedingAsserter = TestAsserter.Succeeding<int>();

            // Act
            Validator.Check(new() { FailureHandling = FailureHandling.KeepGoing })
                .That(TestValue.Of(5))
                .Satisfies(failingAsserter)
                .That(TestValue.Of(6))
                .Satisfies(succeedingAsserter)
                .Result();

            // Assert
            failingAsserter.TimesCalled.Should().Be(1);
            succeedingAsserter.TimesCalled.Should().Be(1);
        }

        [Test]
        public void SkipAssertionsForPropertiesWithSameNameOnErrors_WhenFailureHandlingIsSkipAllForProperty()
        {
            // Arrange
            var failingAsserter = TestAsserter.Failing<int>();
            var succeedingAsserter = TestAsserter.Succeeding<int>();
            var succeedingStringAsserter = TestAsserter.Succeeding<string>();

            // Act
            Validator.Check(new() { FailureHandling = FailureHandling.SkipAllForProperty })
                .That(TestValue.Of(value: 5, name: "five"))
                .Satisfies(failingAsserter)
                .That(TestValue.Of(value: 5, name: "five"))
                .Satisfies(succeedingAsserter)
                .That(TestValue.Of(value: "6", name: "six"))
                .Satisfies(succeedingStringAsserter)
                .Result();

            // Assert
            failingAsserter.TimesCalled.Should().Be(1);
            succeedingAsserter.TimesCalled.Should().Be(0);
            succeedingStringAsserter.TimesCalled.Should().Be(1);
        }

        [Test]
        public void SkipAllRemainingAssertionsOnErrors_WhenFailureHandlingIsSkipAllRemaining()
        {
            // Arrange
            var failingAsserter = TestAsserter.Failing<int>();
            var succeedingAsserter = TestAsserter.Succeeding<int>();

            // Act
            Validator.Check(new() { FailureHandling = FailureHandling.SkipAllRemaining })
                .That(TestValue.Of(5))
                .Satisfies(failingAsserter)
                .That(TestValue.Of(6))
                .Satisfies(succeedingAsserter)
                .Result();

            // Assert
            failingAsserter.TimesCalled.Should().Be(1);
            succeedingAsserter.TimesCalled.Should().Be(0);
        }
    }

    public sealed class OnResult : ValidatorShould
    {
        [Test]
        public void ReturnSuccess_WhenNoAssertionsAreDefined()
        {
            // Act
            var result = Validator.Check().Result();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.IsError.Should().BeFalse();
            result.Errors.Should().BeEmpty();
        }

        [Test]
        public void ReturnSuccess_WhenAllAssertionsSucceed()
        {
            // Arrange
            var assertion = TestAsserter.Succeeding<bool>();

            // Act
            var result = Validator.Check().That(TestValue.Of(true)).Satisfies(assertion).Result();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.IsError.Should().BeFalse();
            result.Errors.Should().BeEmpty();
        }

        [Test]
        public void ReturnFailure_WhenAssertionFailed()
        {
            // Arrange
            var assertion = TestAsserter.Failing<bool>();

            // Act
            var valueProvider = TestValue.Of(true);
            var result = Validator.Check().That(TestValue.Of(true)).Satisfies(assertion).Result();

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.IsError.Should().BeTrue();
            result.Errors.Should().HaveCount(1);
            result.Errors.Single().Should().Be(assertion.GeneratedError);
            result.Errors.Single().ActualValue.Should().Be(valueProvider.Value.ToString());
            result.Errors.Single().PropertyName.Should().Be(valueProvider.Name);
        }

        [Test]
        public void ReturnFailureWithOverriddenActualValue_WhenAssertableIsFormattedUsingShownAs()
        {
            // Arrange
            var assertion = TestAsserter.Failing<bool>();

            // Act
            const string displayedValue = "bool: true";
            var valueProvider = TestValue.Of(true);
            var result = Validator.Check().That(valueProvider).ShownAs(_ => displayedValue).Satisfies(assertion).Result();

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.IsError.Should().BeTrue();
            result.Errors.Should().HaveCount(1);
            result.Errors.Single().Should().Be(assertion.GeneratedError);
            result.Errors.Single().ActualValue.Should().Be(displayedValue);
            result.Errors.Single().PropertyName.Should().Be(valueProvider.Name);
        }

        [Test]
        public void ReturnFailureWithNullAsActualValue_WhenAssertedValueIsNull()
        {
            // Arrange
            var assertion = TestAsserter.Failing<string>();

            // Act
            var valueProvider = TestValue.Of<string>(null);
            var result = Validator.Check().That(valueProvider).Satisfies(assertion).Result();

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.IsError.Should().BeTrue();
            result.Errors.Should().HaveCount(1);
            result.Errors.Single().Should().Be(assertion.GeneratedError);
            result.Errors.Single().ActualValue.Should().Be("null");
            result.Errors.Single().PropertyName.Should().Be(valueProvider.Name);
        }
    }
}

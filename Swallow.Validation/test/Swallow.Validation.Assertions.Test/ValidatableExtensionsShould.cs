namespace Swallow.Validation;

using System.Linq;
using Exceptions;
using FluentAssertions;
using NUnit.Framework;
using TestUtils;

[TestFixture]
internal class ValidatableExtensionsShould
{
    public sealed class OnElse : ValidatableExtensionsShould
    {
        [Test]
        public void InvokeGivenAction_WhenValidationFails()
        {
            // Arrange
            var actionCalled = false;

            // Act
            Validator.Check().That(() => 5).Satisfies(TestAsserter.Failing<int>()).Else(_ => actionCalled = true);

            // Assert
            actionCalled.Should().BeTrue();
        }

        [Test]
        public void NotInvokeGivenAction_WhenValidationSucceeds()
        {
            // Arrange
            var actionCalled = false;

            // Act
            Validator.Check().That(() => 5).Satisfies(TestAsserter.Succeeding<int>()).Else(_ => actionCalled = true);

            // Assert
            actionCalled.Should().BeFalse();
        }
    }

    public sealed class OnElseThrow : ValidatableExtensionsShould
    {
        [Test]
        public void ThrowValidationException_WhenValidationFailed()
        {
            // Act
            var act = () => Validator.Check().That(() => 5).Satisfies(TestAsserter.Failing<int>()).ElseThrow();

            // Assert
            act.Should().Throw<ValidationException>().Which.Errors.Single().Should().BeOfType<TestValidationError>();
        }

        [Test]
        public void NotThrowValidationException_WhenValidationSucceeds()
        {
            // Act
            var act = () => Validator.Check().That(() => 5).Satisfies(TestAsserter.Succeeding<int>()).ElseThrow();

            // Assert
            act.Should().NotThrow();
        }
    }

    public sealed class OnThat : ValidatableExtensionsShould
    {
        [Test]
        public void UseGivenNameForPropertyNameAndEvaluateFunctionForValue_WhenGivenFunction()
        {
            // Arrange
            var asserter = TestAsserter.Failing<int>();

            // Act
            var result = Validator.Check().That(valueFunc: () => 5, name: "value").Satisfies(asserter).Result();

            // Assert
            asserter.LastCheckedValue.Should().Be(5);
            result.Errors.Single().PropertyName.Should().Be("value");
        }

        [Test]
        public void DeterminePropertyNameViaCallerArgumentExpressionAttribute_WhenGivenFunction()
        {
            // Arrange
            var asserter = TestAsserter.Failing<int>();

            // Act
            var result = Validator.Check().That(() => 5).Satisfies(asserter).Result();

            // Assert
            asserter.LastCheckedValue.Should().Be(5);
            result.Errors.Single().PropertyName.Should().Be("5");
        }

        [Test]
        public void UseGivenNameForPropertyNameAndGivenValueForValue_WhenGivenValue()
        {
            // Arrange
            var asserter = TestAsserter.Failing<int>();

            // Act
            var result = Validator.Check().That(value: 5, name: "value").Satisfies(asserter).Result();

            // Assert
            asserter.LastCheckedValue.Should().Be(5);
            result.Errors.Single().PropertyName.Should().Be("value");
        }

        [Test]
        public void DeterminePropertyNameViaCallerArgumentExpressionAttribute_WhenGivenValue()
        {
            // Arrange
            var asserter = TestAsserter.Failing<int>();

            // Act
            var result = Validator.Check().That(5).Satisfies(asserter).Result();

            // Assert
            asserter.LastCheckedValue.Should().Be(5);
            result.Errors.Single().PropertyName.Should().Be("5");
        }
    }

    public sealed class OnThen : ValidatableExtensionsShould
    {
        [Test]
        public void ReturnResultOfFunction_WhenValidationSucceeds()
        {
            // Act
            var result = Validator.Check().That(() => 5).Satisfies(TestAsserter.Succeeding<int>()).Then(() => "hello world");

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be("hello world");
        }

        [Test]
        public void ReturnValidationError_WhenValidationFails()
        {
            // Act
            var result = Validator.Check().That(() => 5).Satisfies(TestAsserter.Failing<int>()).Then(() => "hello world");

            // Assert
            result.IsError.Should().BeTrue();
            result.Errors.Should().HaveCount(1);
        }
    }
}

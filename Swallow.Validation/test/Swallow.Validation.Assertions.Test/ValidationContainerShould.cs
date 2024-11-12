namespace Swallow.Validation;

using System;
using FluentAssertions;
using NUnit.Framework;
using TestUtils;
using Utils;

[TestFixture]
internal class ValidationContainerShould
{
    public sealed class OnCheck : ValidationContainerShould
    {
        [Test]
        public void ReturnTrue_WhenNoAssertersAreRegistered()
        {
            // Act
            var container = new ValidationContainer();
            var result = container.Check(valueProvider: TestValue.Of<object>(5), error: out var error);

            // Assert
            result.Should().BeTrue();
            error.Should().BeNull();
        }

        [Test]
        public void ReturnTrue_WhenRegisteredAsserterDoesNotMatchTypeOfGivenValue()
        {
            // Arrange
            var container = new ValidationContainer();
            container.Register(TestAsserter.Failing<int>());

            // Act
            var result = container.Check(valueProvider: TestValue.Of<object>("hello"), error: out var error);

            // Assert
            result.Should().BeTrue();
            error.Should().BeNull();
        }

        [Test]
        public void ReturnResultOfRegisteredAsserter_WhenGivenTypeMatchesTypeOfAsserter()
        {
            // Arrange
            var asserter = TestAsserter.Failing<int>();
            var container = new ValidationContainer(new[] { asserter });

            // Act
            var result = container.Check(valueProvider: TestValue.Of<object>(5), error: out var error);

            // Assert
            result.Should().BeFalse();
            error.Should().Be(asserter.GeneratedError);
        }

        [Test]
        public void RunAllRegisteredAssertersForTypeOfGivenValue()
        {
            // Arrange
            var firstAsserter = TestAsserter.Succeeding<int>();
            var secondAsserter = TestAsserter.Succeeding<int>();
            var container = new ValidationContainer(new[] { firstAsserter, secondAsserter });

            // Act
            container.Check(valueProvider: TestValue.Of<object>(5), error: out _);

            // Assert
            firstAsserter.TimesCalled.Should().Be(1);
            secondAsserter.TimesCalled.Should().Be(1);
        }
    }

    public sealed class OnRegister : ValidationContainerShould
    {
        [Test]
        public void ThrowException_WhenGivenObjectIsNotAnAsserter()
        {
            // Act
            var container = new ValidationContainer();
            var act = () => container.Register("hello");

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void RegisterAsserter_WhenGivenAsObject()
        {
            // Arrange
            var asserter = TestAsserter.Succeeding<int>();

            // Act
            var container = new ValidationContainer();
            container.Register((object)asserter);

            // Assert
            AssertionTester.Assert<object>(value: 5, assertion: v => v.Satisfies(container));
            asserter.TimesCalled.Should().Be(1);
        }
    }

    public sealed class OnConstructor : ValidationContainerShould
    {
        [Test]
        public void RegisterTheGivenAsserters()
        {
            // Arrange
            var firstAsserter = TestAsserter.Succeeding<int>();

            // Act
            var container = new ValidationContainer(new object[] { firstAsserter });

            // Assert
            AssertionTester.Assert<object>(value: 5, assertion: v => v.Satisfies(container));
            firstAsserter.TimesCalled.Should().Be(1);
        }

        [Test]
        public void ThrowException_WhenArgumentIsNotAnAsserter()
        {
            // Act
            var act = () => new ValidationContainer(new[] { "hello" });

            // Assert
            act.Should().Throw<ArgumentException>();
        }
    }
}

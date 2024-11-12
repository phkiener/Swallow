namespace Swallow.Validation;

using FluentAssertions;
using NUnit.Framework;
using TestUtils;

[TestFixture]
internal class ConditionableExtensionsShould
{
    public sealed class OnUnless : ConditionableExtensionsShould
    {
        [Test]
        [TestCase(arg1: true, arg2: false)]
        [TestCase(arg1: false, arg2: true)]
        public void RunAssertionOnFalse_WhenGivenAsValue(bool conditionValue, bool asserterShouldBeCalled)
        {
            // Arrange
            var asserter = TestAsserter.Succeeding<int>();

            // Act
            Validator.Check().That(() => 5).Satisfies(asserter).Unless(conditionValue).Result();

            // Assert
            var expectedNumberOfCalls = asserterShouldBeCalled ? 1 : 0;
            asserter.TimesCalled.Should().Be(expectedNumberOfCalls);
        }

        [Test]
        [TestCase(arg1: true, arg2: false)]
        [TestCase(arg1: false, arg2: true)]
        public void RunAssertionOnFalse_WhenGivenAsFunction(bool conditionValue, bool asserterShouldBeCalled)
        {
            // Arrange
            var asserter = TestAsserter.Succeeding<int>();

            // Act
            Validator.Check().That(() => 5).Satisfies(asserter).Unless(() => conditionValue).Result();

            // Assert
            var expectedNumberOfCalls = asserterShouldBeCalled ? 1 : 0;
            asserter.TimesCalled.Should().Be(expectedNumberOfCalls);
        }
    }

    public sealed class OnWhen : ConditionableExtensionsShould
    {
        [Test]
        [TestCase(arg1: true, arg2: true)]
        [TestCase(arg1: false, arg2: false)]
        public void RunAssertionOnTrue_WhenGivenAsValue(bool conditionValue, bool asserterShouldBeCalled)
        {
            // Arrange
            var asserter = TestAsserter.Succeeding<int>();

            // Act
            Validator.Check().That(() => 5).Satisfies(asserter).When(conditionValue).Result();
            var expectedNumberOfCalls = asserterShouldBeCalled ? 1 : 0;
            asserter.TimesCalled.Should().Be(expectedNumberOfCalls);
        }

        [Test]
        [TestCase(arg1: true, arg2: true)]
        [TestCase(arg1: false, arg2: false)]
        public void RunAssertionOnTrue_WhenGivenAsFunction(bool conditionValue, bool asserterShouldBeCalled)
        {
            // Arrange
            var asserter = TestAsserter.Succeeding<int>();

            // Act
            Validator.Check().That(() => 5).Satisfies(asserter).When(() => conditionValue).Result();

            // Assert
            var expectedNumberOfCalls = asserterShouldBeCalled ? 1 : 0;
            asserter.TimesCalled.Should().Be(expectedNumberOfCalls);
        }
    }
}

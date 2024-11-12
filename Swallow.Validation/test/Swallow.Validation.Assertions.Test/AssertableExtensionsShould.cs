namespace Swallow.Validation;

using System.Linq;
using Errors;
using FluentAssertions;
using NUnit.Framework;
using Utils;

[TestFixture]
internal class AssertableExtensionsShould
{
    public sealed class OnSatisfies : AssertableExtensionsShould
    {
        [Test]
        public void ProduceGivenErrorInResult_WhenErrorFunctionIsGiven()
        {
            // Arrange
            var error = RangeValidationError<int>.FromLowerBound(value: 5, isInclusive: false);

            // Act
            var result = AssertionTester.Assert(value: 5, assertion: v => v.Satisfies(predicate: x => x > 5, errorFunc: _ => error));

            // Assert
            result.Errors.Single().Should().Be(error);
        }

        [Test]
        public void ProductGenericValidationError_WhenMessageIsGiven()
        {
            // Act
            var result = AssertionTester.Assert(value: 5, assertion: v => v.Satisfies(predicate: x => x > 5, message: "be larger than 5"));

            // Assert
            result.Should().HaveError<GenericValidationError>().Which.RequiredState.Should().Be("be larger than 5");
        }
    }
}

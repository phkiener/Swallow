namespace Swallow.Validation.Assertions;

using System;
using System.Collections.Generic;
using System.Linq;
using Errors;
using FluentAssertions;
using NUnit.Framework;
using Utils;

[TestFixture]
internal class CollectionExtensionsShould
{
    public sealed class OnHasAll : CollectionExtensionsShould
    {
        [Test]
        public void FailAssertion_WhenNoValuesSatisfyInnerAssertion()
        {
            // Act
            var values = new[] { "1", "2", "3" };
            var result = AssertionTester.Assert<IEnumerable<string>>(value: values, assertion: v => v.HasAll(x => x.Matches("[a-zA-Z]+")));

            // Assert
            result.Should().HaveError<EntityValidationError>();
            var error = result.Errors.Single().As<EntityValidationError>();
            error.ValidationErrors.Should().HaveCount(3);
            error.ValidationErrors[0].Should().BeOfType<RegexValidationError>().Which.PropertyName.Should().Be("value[0]");
            error.ValidationErrors[1].Should().BeOfType<RegexValidationError>().Which.PropertyName.Should().Be("value[1]");
            error.ValidationErrors[2].Should().BeOfType<RegexValidationError>().Which.PropertyName.Should().Be("value[2]");
        }

        [Test]
        public void PassAssertion_WhenAllValuesSatisfyInnerAssertion()
        {
            // Act
            var values = new[] { "alpha", "beta", "gamma" };
            var result = AssertionTester.Assert<IEnumerable<string>>(value: values, assertion: v => v.HasAll(x => x.Matches("[a-zA-Z]+")));

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenOneValueDoesNotSatisfyInnerAssertion()
        {
            // Act
            var values = new[] { "alpha", "2", "gamma" };
            var result = AssertionTester.Assert<IEnumerable<string>>(value: values, assertion: v => v.HasAll(x => x.Matches("[a-zA-Z]+")));

            // Assert
            result.Should().HaveError<EntityValidationError>();
            var error = result.Errors.Single().As<EntityValidationError>();
            error.ValidationErrors.Should().HaveCount(1);
            error.ValidationErrors[0].Should().BeOfType<RegexValidationError>().Which.PropertyName.Should().Be("value[1]");
        }
    }

    public sealed class OnHasAllValid : CollectionExtensionsShould
    {
        [Test]
        public void PassAssertion_WhenAllElementsAreValid()
        {
            // Act
            var values = new[] { new TestValidatableEntity { ShouldSucceed = true }, new TestValidatableEntity { ShouldSucceed = true } };
            var result = AssertionTester.Assert<IEnumerable<TestValidatableEntity>>(value: values, assertion: v => v.HasAllValid());

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailValidation_WhenOneElementIsNotValid()
        {
            // Act
            var values = new[] { new TestValidatableEntity { ShouldSucceed = false }, new TestValidatableEntity { ShouldSucceed = true } };
            var result = AssertionTester.Assert<IEnumerable<TestValidatableEntity>>(value: values, assertion: v => v.HasAllValid());

            // Assert
            result.Should().HaveError<EntityValidationError>();
            var error = result.Errors.Single().As<EntityValidationError>();
            error.ValidationErrors.Should().HaveCount(1);
            error.ValidationErrors[0].Should().BeOfType<EntityValidationError>().Which.PropertyName.Should().Be("value[0]");
        }
    }

    public sealed class OnIsIn : CollectionExtensionsShould
    {
        [Test]
        public void PassAssertion_WhenValueIsInGivenSet()
        {
            // Act
            var result = AssertionTester.Assert(value: "alpha", assertion: v => v.IsIn(new[] { "alpha", "beta", "gamma" }));

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenValueIsNotInGivenSet()
        {
            // Act
            var result = AssertionTester.Assert(value: "delta", assertion: v => v.IsIn(new[] { "alpha", "beta", "gamma" }));

            // Assert
            result.Should().HaveError<DisallowedValueValidationError<string>>().Which.ShouldNotMatch.Should().BeFalse();
        }
    }

    public sealed class OnIsNotEmpty : CollectionExtensionsShould
    {
        [Test]
        public void PassAssertion_WhenValueContainsAnElement()
        {
            // Act
            var result = AssertionTester.Assert<IEnumerable<string>>(value: new[] { "yo" }, assertion: v => v.IsNotEmpty());

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenValueIsEmpty()
        {
            // Act
            var result = AssertionTester.Assert<IEnumerable<string>>(value: Array.Empty<string>(), assertion: v => v.IsNotEmpty());

            // Assert
            result.Should().HaveError<EmptyCollectionValidationError>();
        }
    }

    public sealed class OnIsNotIn : CollectionExtensionsShould
    {
        [Test]
        public void FailAssertion_WhenValueIsInGivenSet()
        {
            // Act
            var result = AssertionTester.Assert(value: "alpha", assertion: v => v.IsNotIn(new[] { "alpha", "beta", "gamma" }));

            // Assert
            result.Should().HaveError<DisallowedValueValidationError<string>>().Which.ShouldNotMatch.Should().BeTrue();
        }

        [Test]
        public void PassAssertion_WhenValueIsNotInGivenSet()
        {
            // Act
            var result = AssertionTester.Assert(value: "delta", assertion: v => v.IsNotIn(new[] { "alpha", "beta", "gamma" }));

            // Assert
            result.Should().BeSuccess();
        }
    }
}

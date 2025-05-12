namespace Swallow.Validation.Assertions;

using System;
using System.Linq;
using Errors;
using FluentAssertions;
using NUnit.Framework;
using Utils;

[TestFixture]
internal class ComparisonExtensionsShould
{
    public sealed class OnIsEqualTo : ComparisonExtensionsShould
    {
        [Test]
        public void PassAssertion_WhenValuesAreEqual()
        {
            // Act
            var result = AssertionTester.Assert(value: 12, assertion: v => v.IsEqualTo(12));

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenValuesAreDifferent()
        {
            // Act
            var result = AssertionTester.Assert(value: 12, assertion: v => v.IsEqualTo(2));

            // Assert
            result.Should().HaveError<EqualityValidationError<int>>();
            var error = result.Errors.Single().As<EqualityValidationError<int>>();
            error.ExpectedValue.Should().Be(2);
            error.ShouldNotBeEqual.Should().BeFalse();
        }
    }

    public sealed class OnIsEqualToNullable : ComparisonExtensionsShould
    {
        [Test]
        public void FailAssertion_WhenAssertedValueIsNull()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: null, assertion: v => v.IsEqualTo(2));

            // Assert
            result.Should().HaveError<EqualityValidationError<int>>();
            var error = result.Errors.Single().As<EqualityValidationError<int>>();
            error.ExpectedValue.Should().Be(2);
            error.ShouldNotBeEqual.Should().BeFalse();
        }

        [Test]
        public void PassAssertion_WhenValuesAreEqual()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: 12, assertion: v => v.IsEqualTo(12));

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenValuesAreDifferent()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: 12, assertion: v => v.IsEqualTo(2));

            // Assert
            result.Should().HaveError<EqualityValidationError<int>>();
            var error = result.Errors.Single().As<EqualityValidationError<int>>();
            error.ExpectedValue.Should().Be(2);
            error.ShouldNotBeEqual.Should().BeFalse();
        }
    }

    public sealed class OnIsGreaterThan : ComparisonExtensionsShould
    {
        [Test]
        public void FailAssertion_WhenAssertedValueIsLessThanGivenValue()
        {
            // Act
            var result = AssertionTester.Assert(value: 5, assertion: v => v.IsGreaterThan(10));

            // Assert
            result.Should().HaveError<RangeValidationError<int>>().Which.LowerBound.Should().Be(10);
        }

        [Test]
        public void FailAssertion_WhenAssertedValueIsEqualToGivenValue()
        {
            // Act
            var result = AssertionTester.Assert(value: 10, assertion: v => v.IsGreaterThan(10));

            // Assert
            result.Should().HaveError<RangeValidationError<int>>().Which.LowerBound.Should().Be(10);
        }

        [Test]
        public void PassAssertion_WhenAssertedValueIsGreaterThanGivenValue()
        {
            // Act
            var result = AssertionTester.Assert(value: 15, assertion: v => v.IsGreaterThan(10));

            // Assert
            result.Should().BeSuccess();
        }
    }

    public sealed class OnIsGreaterThanNullable : ComparisonExtensionsShould
    {
        [Test]
        public void FailAssertion_WhenAssertedValueIsNull()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: null, assertion: v => v.IsGreaterThan(10));

            // Assert
            result.Should().HaveError<RangeValidationError<int>>().Which.LowerBound.Should().Be(10);
        }

        [Test]
        public void FailAssertion_WhenAssertedValueIsLessThanGivenValue()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: 5, assertion: v => v.IsGreaterThan(10));

            // Assert
            result.Should().HaveError<RangeValidationError<int>>().Which.LowerBound.Should().Be(10);
        }

        [Test]
        public void FailAssertion_WhenAssertedValueIsEqualToGivenValue()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: 10, assertion: v => v.IsGreaterThan(10));

            // Assert
            result.Should().HaveError<RangeValidationError<int>>().Which.LowerBound.Should().Be(10);
        }

        [Test]
        public void PassAssertion_WhenAssertedValueIsGreaterThanGivenValue()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: 15, assertion: v => v.IsGreaterThan(10));

            // Assert
            result.Should().BeSuccess();
        }
    }

    public sealed class OnIsGreaterThanOrEqual : ComparisonExtensionsShould
    {
        [Test]
        public void FailAssertion_WhenAssertedValueIsLessThanGivenValue()
        {
            // Act
            var result = AssertionTester.Assert(value: 5, assertion: v => v.IsGreaterThanOrEqual(10));

            // Assert
            result.Should().HaveError<RangeValidationError<int>>().Which.LowerBound.Should().Be(10);
        }

        [Test]
        public void PassAssertion_WhenAssertedValueIsEqualToGivenValue()
        {
            // Act
            var result = AssertionTester.Assert(value: 10, assertion: v => v.IsGreaterThanOrEqual(10));

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void PassAssertion_WhenAssertedValueIsGreaterThanGivenValue()
        {
            // Act
            var result = AssertionTester.Assert(value: 15, assertion: v => v.IsGreaterThanOrEqual(10));

            // Assert
            result.Should().BeSuccess();
        }
    }

    public sealed class OnIsGreaterThanOrEqualNullable : ComparisonExtensionsShould
    {
        [Test]
        public void FailAssertion_WhenAssertedValueIsNull()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: null, assertion: v => v.IsGreaterThanOrEqual(10));

            // Assert
            result.Should().HaveError<RangeValidationError<int>>().Which.LowerBound.Should().Be(10);
        }

        [Test]
        public void FailAssertion_WhenAssertedValueIsLessThanGivenValue()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: 5, assertion: v => v.IsGreaterThanOrEqual(10));

            // Assert
            result.Should().HaveError<RangeValidationError<int>>().Which.LowerBound.Should().Be(10);
        }

        [Test]
        public void PassAssertion_WhenAssertedValueIsEqualToGivenValue()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: 10, assertion: v => v.IsGreaterThanOrEqual(10));

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void PassAssertion_WhenAssertedValueIsGreaterThanGivenValue()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: 15, assertion: v => v.IsGreaterThanOrEqual(10));

            // Assert
            result.Should().BeSuccess();
        }
    }

    public sealed class OnIsInRange : ComparisonExtensionsShould
    {
        [Test]
        public void PassAssertion_WhenAssertedValueIsInGivenRange()
        {
            // Act
            var result = AssertionTester.Assert(value: 100, assertion: v => v.IsInRange(lowerInclusive: 50, upperInclusive: 500));

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenAssertedValueIsOutsideOfGivenRange()
        {
            // Act
            var result = AssertionTester.Assert(value: 5000, assertion: v => v.IsInRange(lowerInclusive: 50, upperInclusive: 500));

            // Assert
            result.Should().HaveError<RangeValidationError<int>>();
            var error = result.Errors.Single().As<RangeValidationError<int>>();
            error.LowerBound.Should().Be(50);
            error.UpperBound.Should().Be(500);
        }

        [Test]
        public void PassAssertion_WhenAssertedValueIsEqualToLowerBound()
        {
            // Act
            var result = AssertionTester.Assert(value: 50, assertion: v => v.IsInRange(lowerInclusive: 50, upperInclusive: 500));

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void PassAssertion_WhenAssertedValueIsEqualToUpperBound()
        {
            // Act
            var result = AssertionTester.Assert(value: 500, assertion: v => v.IsInRange(lowerInclusive: 50, upperInclusive: 500));

            // Assert
            result.Should().BeSuccess();
        }
    }

    public sealed class OnIsInRangeNullable : ComparisonExtensionsShould
    {
        [Test]
        public void FailAssertion_WhenAssertedValueIsNull()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: null, assertion: v => v.IsInRange(lowerInclusive: 50, upperInclusive: 500));

            // Assert
            result.Should().HaveError<RangeValidationError<int>>();
        }

        [Test]
        public void PassAssertion_WhenAssertedValueIsInGivenRange()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: 100, assertion: v => v.IsInRange(lowerInclusive: 50, upperInclusive: 500));

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenAssertedValueIsOutsideOfGivenRange()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: 5000, assertion: v => v.IsInRange(lowerInclusive: 50, upperInclusive: 500));

            // Assert
            result.Should().HaveError<RangeValidationError<int>>();
            var error = result.Errors.Single().As<RangeValidationError<int>>();
            error.LowerBound.Should().Be(50);
            error.UpperBound.Should().Be(500);
        }

        [Test]
        public void PassAssertion_WhenAssertedValueIsEqualToLowerBound()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: 50, assertion: v => v.IsInRange(lowerInclusive: 50, upperInclusive: 500));

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void PassAssertion_WhenAssertedValueIsEqualToUpperBound()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: 500, assertion: v => v.IsInRange(lowerInclusive: 50, upperInclusive: 500));

            // Assert
            result.Should().BeSuccess();
        }
    }

    public sealed class OnIsLessThan : ComparisonExtensionsShould
    {
        [Test]
        public void FailAssertion_WhenAssertedValueIsGreaterThanGivenValue()
        {
            // Act
            var result = AssertionTester.Assert(value: 15, assertion: v => v.IsLessThan(10));

            // Assert
            result.Should().HaveError<RangeValidationError<int>>().Which.UpperBound.Should().Be(10);
        }

        [Test]
        public void FailAssertion_WhenAssertedValueIsEqualToGivenValue()
        {
            // Act
            var result = AssertionTester.Assert(value: 10, assertion: v => v.IsLessThan(10));

            // Assert
            result.Should().HaveError<RangeValidationError<int>>().Which.UpperBound.Should().Be(10);
        }

        [Test]
        public void PassAssertion_WhenAssertedValueIsLessThanGivenValue()
        {
            // Act
            var result = AssertionTester.Assert(value: 5, assertion: v => v.IsLessThan(10));

            // Assert
            result.Should().BeSuccess();
        }
    }

    public sealed class OnIsLessThanNullable : ComparisonExtensionsShould
    {
        [Test]
        public void FailAssertion_WhenAssertedValueIsNull()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: null, assertion: v => v.IsLessThan(10));

            // Assert
            result.Should().HaveError<RangeValidationError<int>>().Which.UpperBound.Should().Be(10);
        }

        [Test]
        public void FailAssertion_WhenAssertedValueIsGreaterThanGivenValue()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: 15, assertion: v => v.IsLessThan(10));

            // Assert
            result.Should().HaveError<RangeValidationError<int>>().Which.UpperBound.Should().Be(10);
        }

        [Test]
        public void FailAssertion_WhenAssertedValueIsEqualToGivenValue()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: 10, assertion: v => v.IsLessThan(10));

            // Assert
            result.Should().HaveError<RangeValidationError<int>>().Which.UpperBound.Should().Be(10);
        }

        [Test]
        public void PassAssertion_WhenAssertedValueIsLessThanGivenValue()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: 5, assertion: v => v.IsLessThan(10));

            // Assert
            result.Should().BeSuccess();
        }
    }

    public sealed class OnIsLessThanOrEqual : ComparisonExtensionsShould
    {
        [Test]
        public void FailAssertion_WhenAssertedValueIsGreaterThanGivenValue()
        {
            // Act
            var result = AssertionTester.Assert(value: 15, assertion: v => v.IsLessThanOrEqual(10));

            // Assert
            result.Should().HaveError<RangeValidationError<int>>().Which.UpperBound.Should().Be(10);
        }

        [Test]
        public void PassAssertion_WhenAssertedValueIsEqualToGivenValue()
        {
            // Act
            var result = AssertionTester.Assert(value: 10, assertion: v => v.IsLessThanOrEqual(10));

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void PassAssertion_WhenAssertedValueIsLessThanGivenValue()
        {
            // Act
            var result = AssertionTester.Assert(value: 5, assertion: v => v.IsLessThanOrEqual(10));

            // Assert
            result.Should().BeSuccess();
        }
    }

    public sealed class OnIsLessThanOrEqualNullable : ComparisonExtensionsShould
    {
        [Test]
        public void FailAssertion_WhenAssertedValueIsNull()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: null, assertion: v => v.IsLessThanOrEqual(10));

            // Assert
            result.Should().HaveError<RangeValidationError<int>>().Which.UpperBound.Should().Be(10);
        }

        [Test]
        public void FailAssertion_WhenAssertedValueIsGreaterThanGivenValue()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: 15, assertion: v => v.IsLessThanOrEqual(10));

            // Assert
            result.Should().HaveError<RangeValidationError<int>>().Which.UpperBound.Should().Be(10);
        }

        [Test]
        public void PassAssertion_WhenAssertedValueIsEqualToGivenValue()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: 10, assertion: v => v.IsLessThanOrEqual(10));

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void PassAssertion_WhenAssertedValueIsLessThanGivenValue()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: 5, assertion: v => v.IsLessThanOrEqual(10));

            // Assert
            result.Should().BeSuccess();
        }
    }

    public sealed class OnIsNotEqualTo : ComparisonExtensionsShould
    {
        [Test]
        public void FailAssertion_WhenValuesAreEqual()
        {
            // Act
            var result = AssertionTester.Assert(value: 12, assertion: v => v.IsNotEqualTo(12));

            // Assert
            result.Should().HaveError<EqualityValidationError<int>>();
            var error = result.Errors.Single().As<EqualityValidationError<int>>();
            error.ExpectedValue.Should().Be(12);
            error.ShouldNotBeEqual.Should().BeTrue();
        }

        [Test]
        public void PassAssertion_WhenValuesAreNotEqual()
        {
            // Act
            var result = AssertionTester.Assert(value: 12, assertion: v => v.IsNotEqualTo(2));

            // Assert
            result.Should().BeSuccess();
        }
    }

    public sealed class OnIsNotEqualToNullable : ComparisonExtensionsShould
    {
        [Test]
        public void PassAssertion_WhenAssertedValueIsNull()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: null, assertion: v => v.IsNotEqualTo(12));

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenValuesAreEqual()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: 12, assertion: v => v.IsNotEqualTo(12));

            // Assert
            result.Should().HaveError<EqualityValidationError<int>>();
            var error = result.Errors.Single().As<EqualityValidationError<int>>();
            error.ExpectedValue.Should().Be(12);
            error.ShouldNotBeEqual.Should().BeTrue();
        }

        [Test]
        public void PassAssertion_WhenValuesAreNotEqual()
        {
            // Act
            var result = AssertionTester.Assert<int?>(value: 12, assertion: v => v.IsNotEqualTo(2));

            // Assert
            result.Should().BeSuccess();
        }
    }

    public sealed class OnIsNotNull : ComparisonExtensionsShould
    {
        [Test]
        public void PassAssertion_WhenNullableValueTypeIsNotNull()
        {
            // Act
            var result = AssertionTester.Assert<DateTime?>(value: new DateTime(year: 2000, month: 01, day: 01), assertion: v => v.IsNotNull());

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenNullableValueTypeIsNull()
        {
            // Act
            var result = AssertionTester.Assert<DateTime?>(value: null, assertion: v => v.IsNotNull());

            // Assert
            result.Should().HaveError<IsNullValidationError>();
        }

        [Test]
        public void PassAssertion_WhenReferenceTypeIsNotNull()
        {
            // Act
            var result = AssertionTester.Assert(value: "yo", assertion: v => v.IsNotNull());

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenReferenceTypeIsNull()
        {
            // Act
            var result = AssertionTester.Assert<string>(value: null, assertion: v => v.IsNotNull());

            // Assert
            result.Should().HaveError<IsNullValidationError>();
        }
    }
}

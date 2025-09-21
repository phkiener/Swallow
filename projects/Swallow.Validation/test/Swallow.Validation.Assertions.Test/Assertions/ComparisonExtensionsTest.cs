namespace Swallow.Validation.Assertions;

using System;
using System.Linq;
using Errors;
using NUnit.Framework;
using Utils;

[TestFixture]
public sealed class ComparisonExtensionsShould
{
    [Test]
    public void IsEqualTo_PassAssertion_WhenValuesAreEqual()
    {
        var result = AssertionTester.Assert(value: 12, assertion: v => v.IsEqualTo(12));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsEqualTo_FailAssertion_WhenValuesAreDifferent()
    {
        var result = AssertionTester.Assert(value: 12, assertion: v => v.IsEqualTo(2));

        var typedError = result.Errors.OfType<EqualityValidationError<int>>().Single();
        Assert.That(typedError.ExpectedValue, Is.EqualTo(2));
        Assert.That(typedError.ShouldNotBeEqual, Is.False);
    }

    [Test]
    public void IsEqualToNullable_FailAssertion_WhenAssertedValueIsNull()
    {
        var result = AssertionTester.Assert<int?>(value: null, assertion: v => v.IsEqualTo(2));

        var typedError = result.Errors.OfType<EqualityValidationError<int>>().Single();
        Assert.That(typedError.ExpectedValue, Is.EqualTo(2));
        Assert.That(typedError.ShouldNotBeEqual, Is.False);
    }

    [Test]
    public void sEqualToNullable_PassAssertion_WhenValuesAreEqual()
    {
        var result = AssertionTester.Assert<int?>(value: 12, assertion: v => v.IsEqualTo(12));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void sEqualToNullable_FailAssertion_WhenValuesAreDifferent()
    {
        var result = AssertionTester.Assert<int?>(value: 12, assertion: v => v.IsEqualTo(2));

        var typedError = result.Errors.OfType<EqualityValidationError<int>>().Single();
        Assert.That(typedError.ExpectedValue, Is.EqualTo(2));
        Assert.That(typedError.ShouldNotBeEqual, Is.False);
    }

    [Test]
    public void IsGreaterThan_FailAssertion_WhenAssertedValueIsLessThanGivenValue()
    {
        var result = AssertionTester.Assert(value: 5, assertion: v => v.IsGreaterThan(10));

        var typedError = result.Errors.OfType<RangeValidationError<int>>().Single();
        Assert.That(typedError.LowerBound, Is.EqualTo(10));
    }

    [Test]
    public void IsGreaterThan_FailAssertion_WhenAssertedValueIsEqualToGivenValue()
    {
        var result = AssertionTester.Assert(value: 10, assertion: v => v.IsGreaterThan(10));

        var typedError = result.Errors.OfType<RangeValidationError<int>>().Single();
        Assert.That(typedError.LowerBound, Is.EqualTo(10));
    }

    [Test]
    public void IsGreaterThan_PassAssertion_WhenAssertedValueIsGreaterThanGivenValue()
    {
        var result = AssertionTester.Assert(value: 15, assertion: v => v.IsGreaterThan(10));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsGreaterThanNullable_FailAssertion_WhenAssertedValueIsNull()
    {
        var result = AssertionTester.Assert<int?>(value: null, assertion: v => v.IsGreaterThan(10));

        var typedError = result.Errors.OfType<RangeValidationError<int>>().Single();
        Assert.That(typedError.LowerBound, Is.EqualTo(10));
    }

    [Test]
    public void IsGreaterThanNullable_FailAssertion_WhenAssertedValueIsLessThanGivenValue()
    {
        var result = AssertionTester.Assert<int?>(value: 5, assertion: v => v.IsGreaterThan(10));

        var typedError = result.Errors.OfType<RangeValidationError<int>>().Single();
        Assert.That(typedError.LowerBound, Is.EqualTo(10));
    }

    [Test]
    public void IsGreaterThanNullable_FailAssertion_WhenAssertedValueIsEqualToGivenValue()
    {
        var result = AssertionTester.Assert<int?>(value: 10, assertion: v => v.IsGreaterThan(10));

        var typedError = result.Errors.OfType<RangeValidationError<int>>().Single();
        Assert.That(typedError.LowerBound, Is.EqualTo(10));
    }

    [Test]
    public void IsGreaterThanNullable_PassAssertion_WhenAssertedValueIsGreaterThanGivenValue()
    {
        var result = AssertionTester.Assert<int?>(value: 15, assertion: v => v.IsGreaterThan(10));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsGreaterThanOrEqual_FailAssertion_WhenAssertedValueIsLessThanGivenValue()
    {
        var result = AssertionTester.Assert(value: 5, assertion: v => v.IsGreaterThanOrEqual(10));

        var typedError = result.Errors.OfType<RangeValidationError<int>>().Single();
        Assert.That(typedError.LowerBound, Is.EqualTo(10));
    }

    [Test]
    public void IsGreaterThanOrEqual_PassAssertion_WhenAssertedValueIsEqualToGivenValue()
    {
        var result = AssertionTester.Assert(value: 10, assertion: v => v.IsGreaterThanOrEqual(10));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsGreaterThanOrEqual_PassAssertion_WhenAssertedValueIsGreaterThanGivenValue()
    {
        var result = AssertionTester.Assert(value: 15, assertion: v => v.IsGreaterThanOrEqual(10));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsGreaterThanOrEqualNullable_FailAssertion_WhenAssertedValueIsNull()
    {
        var result = AssertionTester.Assert<int?>(value: null, assertion: v => v.IsGreaterThanOrEqual(10));

        var typedError = result.Errors.OfType<RangeValidationError<int>>().Single();
        Assert.That(typedError.LowerBound, Is.EqualTo(10));
    }

    [Test]
    public void IsGreaterThanOrEqualNullable_FailAssertion_WhenAssertedValueIsLessThanGivenValue()
    {
        var result = AssertionTester.Assert<int?>(value: 5, assertion: v => v.IsGreaterThanOrEqual(10));

        var typedError = result.Errors.OfType<RangeValidationError<int>>().Single();
        Assert.That(typedError.LowerBound, Is.EqualTo(10));
    }

    [Test]
    public void IsGreaterThanOrEqualNullable_PassAssertion_WhenAssertedValueIsEqualToGivenValue()
    {
        var result = AssertionTester.Assert<int?>(value: 10, assertion: v => v.IsGreaterThanOrEqual(10));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsGreaterThanOrEqualNullable_PassAssertion_WhenAssertedValueIsGreaterThanGivenValue()
    {
        var result = AssertionTester.Assert<int?>(value: 15, assertion: v => v.IsGreaterThanOrEqual(10));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsInRange_PassAssertion_WhenAssertedValueIsInGivenRange()
    {
        var result = AssertionTester.Assert(value: 100, assertion: v => v.IsInRange(lowerInclusive: 50, upperInclusive: 500));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsInRange_FailAssertion_WhenAssertedValueIsOutsideOfGivenRange()
    {
        // Act
        var result = AssertionTester.Assert(value: 5000, assertion: v => v.IsInRange(lowerInclusive: 50, upperInclusive: 500));

        var typedError = result.Errors.OfType<RangeValidationError<int>>().Single();
        Assert.That(typedError.LowerBound, Is.EqualTo(50));
        Assert.That(typedError.UpperBound, Is.EqualTo(500));
    }

    [Test]
    public void IsInRange_PassAssertion_WhenAssertedValueIsEqualToLowerBound()
    {
        var result = AssertionTester.Assert(value: 50, assertion: v => v.IsInRange(lowerInclusive: 50, upperInclusive: 500));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsInRange_PassAssertion_WhenAssertedValueIsEqualToUpperBound()
    {
        var result = AssertionTester.Assert(value: 500, assertion: v => v.IsInRange(lowerInclusive: 50, upperInclusive: 500));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsInRangeNullable_FailAssertion_WhenAssertedValueIsNull()
    {
        var result = AssertionTester.Assert<int?>(value: null, assertion: v => v.IsInRange(lowerInclusive: 50, upperInclusive: 500));
        Assert.That(result.Errors, Has.One.InstanceOf<RangeValidationError<int>>());
    }

    [Test]
    public void IsInRangeNullable_PassAssertion_WhenAssertedValueIsInGivenRange()
    {
        var result = AssertionTester.Assert<int?>(value: 100, assertion: v => v.IsInRange(lowerInclusive: 50, upperInclusive: 500));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsInRangeNullable_FailAssertion_WhenAssertedValueIsOutsideOfGivenRange()
    {
        var result = AssertionTester.Assert<int?>(value: 5000, assertion: v => v.IsInRange(lowerInclusive: 50, upperInclusive: 500));

        var typedError = result.Errors.OfType<RangeValidationError<int>>().Single();
        Assert.That(typedError.LowerBound, Is.EqualTo(50));
        Assert.That(typedError.UpperBound, Is.EqualTo(500));
    }

    [Test]
    public void IsInRangeNullable_PassAssertion_WhenAssertedValueIsEqualToLowerBound()
    {
        var result = AssertionTester.Assert<int?>(value: 50, assertion: v => v.IsInRange(lowerInclusive: 50, upperInclusive: 500));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsInRangeNullable_PassAssertion_WhenAssertedValueIsEqualToUpperBound()
    {
        var result = AssertionTester.Assert<int?>(value: 500, assertion: v => v.IsInRange(lowerInclusive: 50, upperInclusive: 500));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsLessThan_FailAssertion_WhenAssertedValueIsGreaterThanGivenValue()
    {
        var result = AssertionTester.Assert(value: 15, assertion: v => v.IsLessThan(10));

        var typedError = result.Errors.OfType<RangeValidationError<int>>().Single();
        Assert.That(typedError.UpperBound, Is.EqualTo(10));
    }

    [Test]
    public void IsLessThan_FailAssertion_WhenAssertedValueIsEqualToGivenValue()
    {
        var result = AssertionTester.Assert(value: 10, assertion: v => v.IsLessThan(10));

        var typedError = result.Errors.OfType<RangeValidationError<int>>().Single();
        Assert.That(typedError.UpperBound, Is.EqualTo(10));
    }

    [Test]
    public void IsLessThan_PassAssertion_WhenAssertedValueIsLessThanGivenValue()
    {
        var result = AssertionTester.Assert(value: 5, assertion: v => v.IsLessThan(10));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsLessThanNullable_FailAssertion_WhenAssertedValueIsNull()
    {
        var result = AssertionTester.Assert<int?>(value: null, assertion: v => v.IsLessThan(10));

        var typedError = result.Errors.OfType<RangeValidationError<int>>().Single();
        Assert.That(typedError.UpperBound, Is.EqualTo(10));
    }

    [Test]
    public void IsLessThanNullable_FailAssertion_WhenAssertedValueIsGreaterThanGivenValue()
    {
        var result = AssertionTester.Assert<int?>(value: 15, assertion: v => v.IsLessThan(10));

        var typedError = result.Errors.OfType<RangeValidationError<int>>().Single();
        Assert.That(typedError.UpperBound, Is.EqualTo(10));
    }

    [Test]
    public void IsLessThanNullable_FailAssertion_WhenAssertedValueIsEqualToGivenValue()
    {
        var result = AssertionTester.Assert<int?>(value: 10, assertion: v => v.IsLessThan(10));

        var typedError = result.Errors.OfType<RangeValidationError<int>>().Single();
        Assert.That(typedError.UpperBound, Is.EqualTo(10));
    }

    [Test]
    public void IsLessThanNullable_PassAssertion_WhenAssertedValueIsLessThanGivenValue()
    {
        var result = AssertionTester.Assert<int?>(value: 5, assertion: v => v.IsLessThan(10));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsLessThanOrEqual_FailAssertion_WhenAssertedValueIsGreaterThanGivenValue()
    {
        var result = AssertionTester.Assert(value: 15, assertion: v => v.IsLessThanOrEqual(10));

        var typedError = result.Errors.OfType<RangeValidationError<int>>().Single();
        Assert.That(typedError.UpperBound, Is.EqualTo(10));
    }

    [Test]
    public void IsLessThanOrEqual_PassAssertion_WhenAssertedValueIsEqualToGivenValue()
    {
        var result = AssertionTester.Assert(value: 10, assertion: v => v.IsLessThanOrEqual(10));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsLessThanOrEqual_PassAssertion_WhenAssertedValueIsLessThanGivenValue()
    {
        var result = AssertionTester.Assert(value: 5, assertion: v => v.IsLessThanOrEqual(10));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsLessThanOrEqualNullable_FailAssertion_WhenAssertedValueIsNull()
    {
        var result = AssertionTester.Assert<int?>(value: null, assertion: v => v.IsLessThanOrEqual(10));

        var typedError = result.Errors.OfType<RangeValidationError<int>>().Single();
        Assert.That(typedError.UpperBound, Is.EqualTo(10));
    }

    [Test]
    public void IsLessThanOrEqualNullable_FailAssertion_WhenAssertedValueIsGreaterThanGivenValue()
    {
        var result = AssertionTester.Assert<int?>(value: 15, assertion: v => v.IsLessThanOrEqual(10));

        var typedError = result.Errors.OfType<RangeValidationError<int>>().Single();
        Assert.That(typedError.UpperBound, Is.EqualTo(10));
    }

    [Test]
    public void IsLessThanOrEqualNullable_PassAssertion_WhenAssertedValueIsEqualToGivenValue()
    {
        var result = AssertionTester.Assert<int?>(value: 10, assertion: v => v.IsLessThanOrEqual(10));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsLessThanOrEqualNullable_PassAssertion_WhenAssertedValueIsLessThanGivenValue()
    {
        var result = AssertionTester.Assert<int?>(value: 5, assertion: v => v.IsLessThanOrEqual(10));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsNotEqualTo_FailAssertion_WhenValuesAreEqual()
    {
        var result = AssertionTester.Assert(value: 12, assertion: v => v.IsNotEqualTo(12));

        var typedError = result.Errors.OfType<EqualityValidationError<int>>().Single();
        Assert.That(typedError.ExpectedValue, Is.EqualTo(12));
        Assert.That(typedError.ShouldNotBeEqual, Is.True);
    }

    [Test]
    public void IsNotEqualTo_PassAssertion_WhenValuesAreNotEqual()
    {
        var result = AssertionTester.Assert(value: 12, assertion: v => v.IsNotEqualTo(2));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsNotEqualToNullable_PassAssertion_WhenAssertedValueIsNull()
    {
        var result = AssertionTester.Assert<int?>(value: null, assertion: v => v.IsNotEqualTo(12));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsNotEqualToNullable_FailAssertion_WhenValuesAreEqual()
    {
        var result = AssertionTester.Assert<int?>(value: 12, assertion: v => v.IsNotEqualTo(12));

        var typedError = result.Errors.OfType<EqualityValidationError<int>>().Single();
        Assert.That(typedError.ExpectedValue, Is.EqualTo(12));
        Assert.That(typedError.ShouldNotBeEqual, Is.True);
    }

    [Test]
    public void IsNotEqualToNullable_PassAssertion_WhenValuesAreNotEqual()
    {
        var result = AssertionTester.Assert<int?>(value: 12, assertion: v => v.IsNotEqualTo(2));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsNotNull_PassAssertion_WhenNullableValueTypeIsNotNull()
    {
        var result = AssertionTester.Assert<DateTime?>(value: new DateTime(year: 2000, month: 01, day: 01), assertion: v => v.IsNotNull());
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsNotNull_FailAssertion_WhenNullableValueTypeIsNull()
    {
        var result = AssertionTester.Assert<DateTime?>(value: null, assertion: v => v.IsNotNull());
        Assert.That(result.Errors, Has.One.InstanceOf<IsNullValidationError>());
    }

    [Test]
    public void IsNotNull_PassAssertion_WhenReferenceTypeIsNotNull()
    {
        var result = AssertionTester.Assert(value: "yo", assertion: v => v.IsNotNull());
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsNotNull_FailAssertion_WhenReferenceTypeIsNull()
    {
        var result = AssertionTester.Assert<string>(value: null, assertion: v => v.IsNotNull());
        Assert.That(result.Errors, Has.One.InstanceOf<IsNullValidationError>());
    }
}

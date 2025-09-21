namespace Swallow.Validation.Assertions;

using System;
using Errors;
using NUnit.Framework;
using Utils;

[TestFixture]
public sealed class DateTimeExtensionsTest
{
    private static readonly DateTime TestDate = DateTime.UtcNow;

    [Test]
    public void IsAfter_PassAssertion_WhenGivenTimeIsBeforeAssertedTime()
    {
        var result = AssertionTester.Assert(value: TestDate, assertion: v => v.IsAfter(TestDate.AddDays(-10)));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsAfter_FailAssertion_WhenGivenTimeIsAfterAssertedTime()
    {
        var result = AssertionTester.Assert(value: TestDate, assertion: v => v.IsAfter(TestDate.AddDays(10)));
        Assert.That(result.Errors, Has.One.InstanceOf<RangeValidationError<DateTime>>());
    }

    [Test]
    public void IsAfter_FailAssertion_WhenDatesAreEqual()
    {
        var result = AssertionTester.Assert(value: TestDate, assertion: v => v.IsBefore(TestDate));
        Assert.That(result.Errors, Has.One.InstanceOf<RangeValidationError<DateTime>>());
    }

    [Test]
    public void IsAfterNullable_FailAssertion_WhenGivenTimeIsNull()
    {
        var result = AssertionTester.Assert<DateTime?>(value: null, assertion: v => v.IsAfter(TestDate));
        Assert.That(result.Errors, Has.One.InstanceOf<RangeValidationError<DateTime>>());
    }

    [Test]
    public void IsAfterNullable_FailAssertion_WhenGivenTimeIsBeforeAssertedTime()
    {
        var result = AssertionTester.Assert<DateTime?>(value: TestDate, assertion: v => v.IsAfter(TestDate.AddDays(10)));
        Assert.That(result.Errors, Has.One.InstanceOf<RangeValidationError<DateTime>>());
    }

    [Test]
    public void IsAfterNullable_PassAssertion_WhenGivenTimeIsBeforeAssertedTime()
    {
        var result = AssertionTester.Assert<DateTime?>(value: TestDate, assertion: v => v.IsAfter(TestDate.AddDays(-10)));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsAfterNullable_FailAssertion_WhenDatesAreEqual()
    {
        var result = AssertionTester.Assert<DateTime?>(value: TestDate, assertion: v => v.IsAfter(TestDate));
        Assert.That(result.Errors, Has.One.InstanceOf<RangeValidationError<DateTime>>());
    }

    [Test]
    public void IsBefore_PassAssertion_WhenGivenTimeIsAfterAssertedTime()
    {
        var result = AssertionTester.Assert(value: TestDate, assertion: v => v.IsBefore(TestDate.AddDays(-10)));
        Assert.That(result.Errors, Has.One.InstanceOf<RangeValidationError<DateTime>>());
    }

    [Test]
    public void IsBefore_FailAssertion_WhenGivenTimeIsBeforeAssertedTime()
    {
        var result = AssertionTester.Assert(value: TestDate, assertion: v => v.IsBefore(TestDate.AddDays(10)));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsBefore_FailAssertion_WhenDatesAreEqual()
    {
        var result = AssertionTester.Assert(value: TestDate, assertion: v => v.IsBefore(TestDate));
        Assert.That(result.Errors, Has.One.InstanceOf<RangeValidationError<DateTime>>());
    }

    [Test]
    public void IsBeforeNullable_FailAssertion_WhenGivenTimeIsNull()
    {
        var result = AssertionTester.Assert<DateTime?>(value: null, assertion: v => v.IsBefore(TestDate));
        Assert.That(result.Errors, Has.One.InstanceOf<RangeValidationError<DateTime>>());
    }

    [Test]
    public void IsBeforeNullable_FailAssertion_WhenGivenTimeIsAfterAssertedTime()
    {
        var result = AssertionTester.Assert<DateTime?>(value: TestDate, assertion: v => v.IsBefore(TestDate.AddDays(-10)));
        Assert.That(result.Errors, Has.One.InstanceOf<RangeValidationError<DateTime>>());
    }

    [Test]
    public void IsBeforeNullable_PassAssertion_WhenGivenTimeIsBeforeAssertedTime()
    {
        var result = AssertionTester.Assert<DateTime?>(value: TestDate, assertion: v => v.IsBefore(TestDate.AddDays(10)));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsBeforeNullable_FailAssertion_WhenDatesAreEqual()
    {
        var result = AssertionTester.Assert<DateTime?>(value: TestDate, assertion: v => v.IsBefore(TestDate));
        Assert.That(result.Errors, Has.One.InstanceOf<RangeValidationError<DateTime>>());
    }

    [Test]
    public void IsBetween_PassAssertion_WhenAssertedTimeIsBetweenGivenTimes()
    {
        var lowerBound = TestDate.AddDays(-10);
        var upperBound = TestDate.AddDays(10);

        var result = AssertionTester.Assert(value: TestDate, assertion: v => v.IsBetween(firstDate: lowerBound, secondDate: upperBound));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsBetween_FailAssertion_WhenAssertedTimeIsBeforeLowerBound()
    {
        var lowerBound = TestDate.AddDays(10);
        var upperBound = TestDate.AddDays(20);

        var result = AssertionTester.Assert(value: TestDate, assertion: v => v.IsBetween(firstDate: lowerBound, secondDate: upperBound));
        Assert.That(result.Errors, Has.One.InstanceOf<RangeValidationError<DateTime>>());
    }

    [Test]
    public void IsBetween_FailAssertion_WhenAssertedTimeIsAfterUpperBound()
    {
        var lowerBound = TestDate.AddDays(-20);
        var upperBound = TestDate.AddDays(-10);

        var result = AssertionTester.Assert(value: TestDate, assertion: v => v.IsBetween(firstDate: lowerBound, secondDate: upperBound));
        Assert.That(result.Errors, Has.One.InstanceOf<RangeValidationError<DateTime>>());
    }

    [Test]
    public void IsBetweenNullable_FailAssertion_WhenAssertedTimeIsNull()
    {
        var lowerBound = TestDate.AddDays(10);
        var upperBound = TestDate.AddDays(20);

        var result = AssertionTester.Assert<DateTime?>(value: null, assertion: v => v.IsBetween(firstDate: lowerBound, secondDate: upperBound));
        Assert.That(result.Errors, Has.One.InstanceOf<RangeValidationError<DateTime>>());
    }

    [Test]
    public void IsBetweenNullable_PassAssertion_WhenAssertedTimeIsBetweenGivenTimes()
    {
        var lowerBound = TestDate.AddDays(-10);
        var upperBound = TestDate.AddDays(10);

        var result = AssertionTester.Assert<DateTime?>(
            value: TestDate,
            assertion: v => v.IsBetween(firstDate: lowerBound, secondDate: upperBound));

        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsBetweenNullable_FailAssertion_WhenAssertedTimeIsBeforeLowerBound()
    {
        var lowerBound = TestDate.AddDays(10);
        var upperBound = TestDate.AddDays(20);

        var result = AssertionTester.Assert<DateTime?>(
            value: TestDate,
            assertion: v => v.IsBetween(firstDate: lowerBound, secondDate: upperBound));

        Assert.That(result.Errors, Has.One.InstanceOf<RangeValidationError<DateTime>>());
    }

    [Test]
    public void IsBetweenNullable_FailAssertion_WhenAssertedTimeIsAfterUpperBound()
    {
        var lowerBound = TestDate.AddDays(-20);
        var upperBound = TestDate.AddDays(-10);

        var result = AssertionTester.Assert<DateTime?>(
            value: TestDate,
            assertion: v => v.IsBetween(firstDate: lowerBound, secondDate: upperBound));

        Assert.That(result.Errors, Has.One.InstanceOf<RangeValidationError<DateTime>>());
    }

    [Test]
    public void IsLocalTime_PassAssertion_WhenAssertedTimeIsLocal()
    {
        var result = AssertionTester.Assert(
            value: new DateTime(ticks: TestDate.Ticks, kind: DateTimeKind.Local),
            assertion: v => v.IsLocalTime());

        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsLocalTime_FailAssertion_WhenAssertedTimeHasNoDateTimeKind()
    {
        var result = AssertionTester.Assert(
            value: new DateTime(ticks: TestDate.Ticks, kind: DateTimeKind.Unspecified),
            assertion: v => v.IsLocalTime());

        var typedError = result.Errors.OfType<TimeZoneValidationError>().Single();
        Assert.That(typedError.ActualTimeZone, Is.Null);
    }

    [Test]
    public void IsLocalTime_FailAssertion_WhenAssertedTimeIsUtc()
    {
        var result = AssertionTester.Assert(value: new DateTime(ticks: TestDate.Ticks, kind: DateTimeKind.Utc), assertion: v => v.IsLocalTime());

        var typedError = result.Errors.OfType<TimeZoneValidationError>().Single();
        Assert.That(typedError.ActualTimeZone, Is.EqualTo(TimeZoneInfo.Utc));
    }

    [Test]
    public void IsLocalTimeNullable_FailAssertion_WhenAssertedTimeIsNull()
    {
        var result = AssertionTester.Assert<DateTime?>(value: null, assertion: v => v.IsLocalTime());

        var typedError = result.Errors.OfType<TimeZoneValidationError>().Single();
        Assert.That(typedError.ActualTimeZone, Is.Null);
    }

    [Test]
    public void IsLocalTimeNullable_PassAssertion_WhenAssertedTimeIsLocal()
    {
        var result = AssertionTester.Assert<DateTime?>(
            value: new DateTime(ticks: TestDate.Ticks, kind: DateTimeKind.Local),
            assertion: v => v.IsLocalTime());

        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsLocalTimeNullable_FailAssertion_WhenAssertedTimeHasNoDateTimeKind()
    {
        var result = AssertionTester.Assert<DateTime?>(
            value: new DateTime(ticks: TestDate.Ticks, kind: DateTimeKind.Unspecified),
            assertion: v => v.IsLocalTime());

        var typedError = result.Errors.OfType<TimeZoneValidationError>().Single();
        Assert.That(typedError.ActualTimeZone, Is.Null);
    }

    [Test]
    public void IsLocalTimeNullable_FailAssertion_WhenAssertedTimeIsUtc()
    {
        var result = AssertionTester.Assert<DateTime?>(
            value: new DateTime(ticks: TestDate.Ticks, kind: DateTimeKind.Utc),
            assertion: v => v.IsLocalTime());

        var typedError = result.Errors.OfType<TimeZoneValidationError>().Single();
        Assert.That(typedError.ActualTimeZone, Is.EqualTo(TimeZoneInfo.Utc));
    }

    [Test]
    public void IsUtc_PassAssertion_WhenAssertedTimeIsUtc()
    {
        var result = AssertionTester.Assert<DateTime?>(
            value: new DateTime(ticks: TestDate.Ticks, kind: DateTimeKind.Utc),
            assertion: v => v.IsUtc());

        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsUtc_FailAssertion_WhenAssertedTimeIsLocal()
    {
        var result = AssertionTester.Assert(value: new DateTime(ticks: TestDate.Ticks, kind: DateTimeKind.Local), assertion: v => v.IsUtc());

        var typedError = result.Errors.OfType<TimeZoneValidationError>().Single();
        Assert.That(typedError.ActualTimeZone, Is.EqualTo(TimeZoneInfo.Local));
    }

    [Test]
    public void IsUtc_FailAssertion_WhenAssertedTimeHasNoDateTimeKind()
    {
        var result = AssertionTester.Assert(
            value: new DateTime(ticks: TestDate.Ticks, kind: DateTimeKind.Unspecified),
            assertion: v => v.IsUtc());

        var typedError = result.Errors.OfType<TimeZoneValidationError>().Single();
        Assert.That(typedError.ActualTimeZone, Is.Null);
    }

    [Test]
    public void IsUtcNullable_FailAssertion_WhenAssertedTimeIsNull()
    {
        var result = AssertionTester.Assert<DateTime?>(value: null, assertion: v => v.IsUtc());

        var typedError = result.Errors.OfType<TimeZoneValidationError>().Single();
        Assert.That(typedError.ActualTimeZone, Is.Null);
    }

    [Test]
    public void IsUtcNullable_PassAssertion_WhenAssertedTimeIsUtc()
    {
        var result = AssertionTester.Assert<DateTime?>(
            value: new DateTime(ticks: TestDate.Ticks, kind: DateTimeKind.Utc),
            assertion: v => v.IsUtc());

        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsUtcNullable_FailAssertion_WhenAssertedTimeHasNoDateTimeKind()
    {
        var result = AssertionTester.Assert<DateTime?>(
            value: new DateTime(ticks: TestDate.Ticks, kind: DateTimeKind.Unspecified),
            assertion: v => v.IsUtc());

        var typedError = result.Errors.OfType<TimeZoneValidationError>().Single();
        Assert.That(typedError.ActualTimeZone, Is.Null);
    }

    [Test]
    public void IsUtcNullable_FailAssertion_WhenAssertedTimeIsLocal()
    {
        var result = AssertionTester.Assert<DateTime?>(
            value: new DateTime(ticks: TestDate.Ticks, kind: DateTimeKind.Local),
            assertion: v => v.IsUtc());

        var typedError = result.Errors.OfType<TimeZoneValidationError>().Single();
        Assert.That(typedError.ActualTimeZone, Is.EqualTo(TimeZoneInfo.Local));
    }
}

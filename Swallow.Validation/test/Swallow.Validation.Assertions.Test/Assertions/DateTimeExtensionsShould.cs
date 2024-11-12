namespace Swallow.Validation.Assertions;

using System;
using Errors;
using FluentAssertions;
using NUnit.Framework;
using Utils;

[TestFixture]
internal class DateTimeExtensionsShould
{
    private static readonly DateTime TestDate = DateTime.UtcNow;

    public sealed class OnIsAfter : DateTimeExtensionsShould
    {
        [Test]
        public void PassAssertion_WhenGivenTimeIsBeforeAssertedTime()
        {
            // Act
            var result = AssertionTester.Assert(value: TestDate, assertion: v => v.IsAfter(TestDate.AddDays(-10)));

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenGivenTimeIsAfterAssertedTime()
        {
            // Act
            var result = AssertionTester.Assert(value: TestDate, assertion: v => v.IsAfter(TestDate.AddDays(10)));

            // Assert
            result.Should().HaveError<RangeValidationError<DateTime>>();
        }

        [Test]
        public void FailAssertion_WhenDatesAreEqual()
        {
            // Act
            var result = AssertionTester.Assert(value: TestDate, assertion: v => v.IsBefore(TestDate));

            // Assert
            result.Should().HaveError<RangeValidationError<DateTime>>();
        }
    }

    public sealed class OnIsAfterNullable : DateTimeExtensionsShould
    {
        [Test]
        public void FailAssertion_WhenGivenTimeIsNull()
        {
            // Act
            var result = AssertionTester.Assert<DateTime?>(value: null, assertion: v => v.IsAfter(TestDate));

            // Assert
            result.Should().HaveError<RangeValidationError<DateTime>>();
        }

        [Test]
        public void FailAssertion_WhenGivenTimeIsBeforeAssertedTime()
        {
            // Act
            var result = AssertionTester.Assert<DateTime?>(value: TestDate, assertion: v => v.IsAfter(TestDate.AddDays(10)));

            // Assert
            result.Should().HaveError<RangeValidationError<DateTime>>();
        }

        [Test]
        public void PassAssertion_WhenGivenTimeIsBeforeAssertedTime()
        {
            // Act
            var result = AssertionTester.Assert<DateTime?>(value: TestDate, assertion: v => v.IsAfter(TestDate.AddDays(-10)));

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenDatesAreEqual()
        {
            // Act
            var result = AssertionTester.Assert<DateTime?>(value: TestDate, assertion: v => v.IsAfter(TestDate));

            // Assert
            result.Should().HaveError<RangeValidationError<DateTime>>();
        }
    }

    public sealed class OnIsBefore : DateTimeExtensionsShould
    {
        [Test]
        public void PassAssertion_WhenGivenTimeIsAfterAssertedTime()
        {
            // Act
            var result = AssertionTester.Assert(value: TestDate, assertion: v => v.IsBefore(TestDate.AddDays(-10)));

            // Assert
            result.Should().HaveError<RangeValidationError<DateTime>>();
        }

        [Test]
        public void FailAssertion_WhenGivenTimeIsBeforeAssertedTime()
        {
            // Act
            var result = AssertionTester.Assert(value: TestDate, assertion: v => v.IsBefore(TestDate.AddDays(10)));

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenDatesAreEqual()
        {
            // Act
            var result = AssertionTester.Assert(value: TestDate, assertion: v => v.IsBefore(TestDate));

            // Assert
            result.Should().HaveError<RangeValidationError<DateTime>>();
        }
    }

    public sealed class OnIsBeforeNullable : DateTimeExtensionsShould
    {
        [Test]
        public void FailAssertion_WhenGivenTimeIsNull()
        {
            // Act
            var result = AssertionTester.Assert<DateTime?>(value: null, assertion: v => v.IsBefore(TestDate));

            // Assert
            result.Should().HaveError<RangeValidationError<DateTime>>();
        }

        [Test]
        public void FailAssertion_WhenGivenTimeIsAfterAssertedTime()
        {
            // Act
            var result = AssertionTester.Assert<DateTime?>(value: TestDate, assertion: v => v.IsBefore(TestDate.AddDays(-10)));

            // Assert
            result.Should().HaveError<RangeValidationError<DateTime>>();
        }

        [Test]
        public void PassAssertion_WhenGivenTimeIsBeforeAssertedTime()
        {
            // Act
            var result = AssertionTester.Assert<DateTime?>(value: TestDate, assertion: v => v.IsBefore(TestDate.AddDays(10)));

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenDatesAreEqual()
        {
            // Act
            var result = AssertionTester.Assert<DateTime?>(value: TestDate, assertion: v => v.IsBefore(TestDate));

            // Assert
            result.Should().HaveError<RangeValidationError<DateTime>>();
        }
    }

    public sealed class OnIsBetween : DateTimeExtensionsShould
    {
        [Test]
        public void PassAssertion_WhenAssertedTimeIsBetweenGivenTimes()
        {
            // Arrange
            var lowerBound = TestDate.AddDays(-10);
            var upperBound = TestDate.AddDays(10);

            // Act
            var result = AssertionTester.Assert(value: TestDate, assertion: v => v.IsBetween(firstDate: lowerBound, secondDate: upperBound));

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenAssertedTimeIsBeforeLowerBound()
        {
            // Arrange
            var lowerBound = TestDate.AddDays(10);
            var upperBound = TestDate.AddDays(20);

            // Act
            var result = AssertionTester.Assert(value: TestDate, assertion: v => v.IsBetween(firstDate: lowerBound, secondDate: upperBound));

            // Assert
            result.Should().HaveError<RangeValidationError<DateTime>>();
        }

        [Test]
        public void FailAssertion_WhenAssertedTimeIsAfterUpperBound()
        {
            // Arrange
            var lowerBound = TestDate.AddDays(-20);
            var upperBound = TestDate.AddDays(-10);

            // Act
            var result = AssertionTester.Assert(value: TestDate, assertion: v => v.IsBetween(firstDate: lowerBound, secondDate: upperBound));

            // Assert
            result.Should().HaveError<RangeValidationError<DateTime>>();
        }
    }

    public sealed class OnIsBetweenNullable : DateTimeExtensionsShould
    {
        [Test]
        public void FailAssertion_WhenAssertedTimeIsNull()
        {
            // Arrange
            var lowerBound = TestDate.AddDays(10);
            var upperBound = TestDate.AddDays(20);

            // Act
            var result = AssertionTester.Assert<DateTime?>(value: null, assertion: v => v.IsBetween(firstDate: lowerBound, secondDate: upperBound));

            // Assert
            result.Should().HaveError<RangeValidationError<DateTime>>();
        }

        [Test]
        public void PassAssertion_WhenAssertedTimeIsBetweenGivenTimes()
        {
            // Arrange
            var lowerBound = TestDate.AddDays(-10);
            var upperBound = TestDate.AddDays(10);

            // Act
            var result = AssertionTester.Assert<DateTime?>(
                value: TestDate,
                assertion: v => v.IsBetween(firstDate: lowerBound, secondDate: upperBound));

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenAssertedTimeIsBeforeLowerBound()
        {
            // Arrange
            var lowerBound = TestDate.AddDays(10);
            var upperBound = TestDate.AddDays(20);

            // Act
            var result = AssertionTester.Assert<DateTime?>(
                value: TestDate,
                assertion: v => v.IsBetween(firstDate: lowerBound, secondDate: upperBound));

            // Assert
            result.Should().HaveError<RangeValidationError<DateTime>>();
        }

        [Test]
        public void FailAssertion_WhenAssertedTimeIsAfterUpperBound()
        {
            // Arrange
            var lowerBound = TestDate.AddDays(-20);
            var upperBound = TestDate.AddDays(-10);

            // Act
            var result = AssertionTester.Assert<DateTime?>(
                value: TestDate,
                assertion: v => v.IsBetween(firstDate: lowerBound, secondDate: upperBound));

            // Assert
            result.Should().HaveError<RangeValidationError<DateTime>>();
        }
    }

    public sealed class OnIsLocalTime : DateTimeExtensionsShould
    {
        [Test]
        public void PassAssertion_WhenAssertedTimeIsLocal()
        {
            // Act
            var result = AssertionTester.Assert(
                value: new DateTime(ticks: TestDate.Ticks, kind: DateTimeKind.Local),
                assertion: v => v.IsLocalTime());

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenAssertedTimeHasNoDateTimeKind()
        {
            // Act
            var result = AssertionTester.Assert(
                value: new DateTime(ticks: TestDate.Ticks, kind: DateTimeKind.Unspecified),
                assertion: v => v.IsLocalTime());

            // Assert
            result.Should().HaveError<TimeZoneValidationError>().Which.ActualTimeZone.Should().BeNull();
        }

        [Test]
        public void FailAssertion_WhenAssertedTimeIsUtc()
        {
            // Act
            var result = AssertionTester.Assert(value: new DateTime(ticks: TestDate.Ticks, kind: DateTimeKind.Utc), assertion: v => v.IsLocalTime());

            // Assert
            result.Should().HaveError<TimeZoneValidationError>().Which.ActualTimeZone.Should().Be(TimeZoneInfo.Utc);
        }
    }

    public sealed class OnIsLocalTimeNullable : DateTimeExtensionsShould
    {
        [Test]
        public void FailAssertion_WhenAssertedTimeIsNull()
        {
            // Act
            var result = AssertionTester.Assert<DateTime?>(value: null, assertion: v => v.IsLocalTime());

            // Assert
            result.Should().HaveError<TimeZoneValidationError>().Which.ActualTimeZone.Should().BeNull();
        }

        [Test]
        public void PassAssertion_WhenAssertedTimeIsLocal()
        {
            // Act
            var result = AssertionTester.Assert<DateTime?>(
                value: new DateTime(ticks: TestDate.Ticks, kind: DateTimeKind.Local),
                assertion: v => v.IsLocalTime());

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenAssertedTimeHasNoDateTimeKind()
        {
            // Act
            var result = AssertionTester.Assert<DateTime?>(
                value: new DateTime(ticks: TestDate.Ticks, kind: DateTimeKind.Unspecified),
                assertion: v => v.IsLocalTime());

            // Assert
            result.Should().HaveError<TimeZoneValidationError>().Which.ActualTimeZone.Should().BeNull();
        }

        [Test]
        public void FailAssertion_WhenAssertedTimeIsUtc()
        {
            // Act
            var result = AssertionTester.Assert<DateTime?>(
                value: new DateTime(ticks: TestDate.Ticks, kind: DateTimeKind.Utc),
                assertion: v => v.IsLocalTime());

            // Assert
            result.Should().HaveError<TimeZoneValidationError>().Which.ActualTimeZone.Should().Be(TimeZoneInfo.Utc);
        }
    }

    public sealed class OnIsUtc : DateTimeExtensionsShould
    {
        [Test]
        public void PassAssertion_WhenAssertedTimeIsUtc()
        {
            // Act
            var result = AssertionTester.Assert<DateTime?>(
                value: new DateTime(ticks: TestDate.Ticks, kind: DateTimeKind.Utc),
                assertion: v => v.IsUtc());

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenAssertedTimeIsLocal()
        {
            // Act
            var result = AssertionTester.Assert(value: new DateTime(ticks: TestDate.Ticks, kind: DateTimeKind.Local), assertion: v => v.IsUtc());

            // Assert
            result.Should().HaveError<TimeZoneValidationError>().Which.ActualTimeZone.Should().Be(TimeZoneInfo.Local);
        }

        [Test]
        public void FailAssertion_WhenAssertedTimeHasNoDateTimeKind()
        {
            // Act
            var result = AssertionTester.Assert(
                value: new DateTime(ticks: TestDate.Ticks, kind: DateTimeKind.Unspecified),
                assertion: v => v.IsUtc());

            // Assert
            result.Should().HaveError<TimeZoneValidationError>().Which.ActualTimeZone.Should().BeNull();
        }
    }

    public sealed class OnIsUtcNullable : DateTimeExtensionsShould
    {
        [Test]
        public void FailAssertion_WhenAssertedTimeIsNull()
        {
            // Act
            var result = AssertionTester.Assert<DateTime?>(value: null, assertion: v => v.IsUtc());

            // Assert
            result.Should().HaveError<TimeZoneValidationError>().Which.ActualTimeZone.Should().BeNull();
        }

        [Test]
        public void PassAssertion_WhenAssertedTimeIsUtc()
        {
            // Act
            var result = AssertionTester.Assert<DateTime?>(
                value: new DateTime(ticks: TestDate.Ticks, kind: DateTimeKind.Utc),
                assertion: v => v.IsUtc());

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenAssertedTimeHasNoDateTimeKind()
        {
            // Act
            var result = AssertionTester.Assert<DateTime?>(
                value: new DateTime(ticks: TestDate.Ticks, kind: DateTimeKind.Unspecified),
                assertion: v => v.IsUtc());

            // Assert
            result.Should().HaveError<TimeZoneValidationError>().Which.ActualTimeZone.Should().BeNull();
        }

        [Test]
        public void FailAssertion_WhenAssertedTimeIsLocal()
        {
            // Act
            var result = AssertionTester.Assert<DateTime?>(
                value: new DateTime(ticks: TestDate.Ticks, kind: DateTimeKind.Local),
                assertion: v => v.IsUtc());

            // Assert
            result.Should().HaveError<TimeZoneValidationError>().Which.ActualTimeZone.Should().Be(TimeZoneInfo.Local);
        }
    }
}

namespace Swallow.Validation.Errors;

using System;
using FluentAssertions;
using NUnit.Framework;

[TestFixture]
internal sealed class TimeZoneValidationErrorShould
{
    [Test]
    public void HaveMessageWithExpectedTimeZone()
    {
        // Arrange
        var error = new TimeZoneValidationError(TimeZoneInfo.Utc) { PropertyName = "Time" };

        // Act
        var message = error.Message;

        // Assert
        message.Should().Be("Time should be in UTC+00:00");
    }

    [Test]
    public void HaveMessageWithOffsetFromUtc_WhenTimezoneIsGiven()
    {
        // Arrange
        var customTimeZone = TimeZoneInfo.CreateCustomTimeZone(
            id: "SVTZI",
            baseUtcOffset: TimeSpan.FromHours(-2.5),
            displayName: null,
            standardDisplayName: null);

        var error = new TimeZoneValidationError(requiredTimeZone: TimeZoneInfo.Utc, actualTimeZone: customTimeZone) { PropertyName = "Time" };

        // Act
        var message = error.Message;

        // Assert
        message.Should().Be("Time should be in UTC+00:00 but was in UTC-02:30");
    }
}

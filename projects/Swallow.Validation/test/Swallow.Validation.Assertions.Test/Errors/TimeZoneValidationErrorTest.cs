namespace Swallow.Validation.Errors;

using System;
using NUnit.Framework;

[TestFixture]
public sealed class TimeZoneValidationErrorTest
{
    [Test]
    public void HaveMessageWithExpectedTimeZone()
    {
        var error = new TimeZoneValidationError(TimeZoneInfo.Utc) { PropertyName = "Time" };
        Assert.That(error.Message, Is.EqualTo("Time should be in UTC+00:00"));
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
        Assert.That(error.Message, Is.EqualTo("Time should be in UTC+00:00 but was in UTC-02:30"));
    }
}

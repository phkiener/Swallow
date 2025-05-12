namespace Swallow.Validation.Errors;

using System;

/// <summary>
///     An error to signal that a time structure is in the wrong timezone.
/// </summary>
public class TimeZoneValidationError : ValidationError
{
    /// <summary>
    ///     Create a <c>TimeZoneValidationError</c>.
    /// </summary>
    /// <param name="requiredTimeZone">The timezone that is required</param>
    /// <param name="actualTimeZone">The actual encountered timezone or null if unspecified.</param>
    public TimeZoneValidationError(TimeZoneInfo requiredTimeZone, TimeZoneInfo actualTimeZone = null)
    {
        RequiredTimeZone = requiredTimeZone;
        ActualTimeZone = actualTimeZone;
    }

    /// <summary>
    ///     Return the required timezone.
    /// </summary>
    public TimeZoneInfo RequiredTimeZone { get; }

    /// <summary>
    ///     Return the actual timezone or null if unspecified.
    /// </summary>
    public TimeZoneInfo ActualTimeZone { get; }

    /// <inheritdoc />
    public override string Message => GetMessage();

    private string GetMessage()
    {
        var actualTimeZone = ActualTimeZone == null ? "" : $" but was in {GetPrettyTimeZoneString(ActualTimeZone)}";

        return $"{PropertyName} should be in {GetPrettyTimeZoneString(RequiredTimeZone)}{actualTimeZone}";
    }

    private static string GetPrettyTimeZoneString(TimeZoneInfo timeZone)
    {
        var offset = timeZone.BaseUtcOffset;
        var positive = offset.TotalHours >= 0;

        return $"UTC{(positive ? "+" : "-")}{Math.Abs(offset.Hours):00}:{Math.Abs(offset.Minutes):00}";
    }
}

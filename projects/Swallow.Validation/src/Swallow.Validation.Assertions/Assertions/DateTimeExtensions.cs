namespace Swallow.Validation.Assertions;

using System;
using Errors;

/// <summary>
///     Extensions for asserting DateTime objects
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    ///     Check that a date is before another date.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="checkDate">The date to compare against.</param>
    /// <returns>The constructed assertion.</returns>
    public static IAssertion<DateTime> IsBefore(this IAssertable<DateTime> assertion, DateTime checkDate)
    {
        return assertion.Satisfies(
            predicate: assertedDate => assertedDate < checkDate,
            errorFunc: _ => RangeValidationError<DateTime>.FromUpperBound(value: checkDate, isInclusive: false));
    }

    /// <summary>
    ///     Check that a date is before another date.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="checkDate">The date to compare against.</param>
    /// <returns>The constructed assertion.</returns>
    /// <remarks>Always fails when the asserted <see cref="DateTime" /> is <c>null</c>.</remarks>
    public static IAssertion<DateTime?> IsBefore(this IAssertable<DateTime?> assertion, DateTime checkDate)
    {
        return assertion.Satisfies(
            predicate: assertedDate => assertedDate < checkDate,
            errorFunc: _ => RangeValidationError<DateTime>.FromUpperBound(value: checkDate, isInclusive: false));
    }

    /// <summary>
    ///     Check that a date is after another date.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="checkDate">The date to compare against.</param>
    /// <returns>The constructed assertion.</returns>
    public static IAssertion<DateTime> IsAfter(this IAssertable<DateTime> assertion, DateTime checkDate)
    {
        return assertion.Satisfies(
            predicate: assertedDate => assertedDate > checkDate,
            errorFunc: _ => RangeValidationError<DateTime>.FromLowerBound(value: checkDate, isInclusive: false));
    }

    /// <summary>
    ///     Check that a date is after another date.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="checkDate">The date to compare against.</param>
    /// <returns>The constructed assertion.</returns>
    /// <remarks>Always fails when the asserted <see cref="DateTime" /> is <c>null</c>.</remarks>
    public static IAssertion<DateTime?> IsAfter(this IAssertable<DateTime?> assertion, DateTime checkDate)
    {
        return assertion.Satisfies(
            predicate: assertedDate => assertedDate > checkDate,
            errorFunc: _ => RangeValidationError<DateTime>.FromLowerBound(value: checkDate, isInclusive: false));
    }

    /// <summary>
    ///     Check that a date is between two dates.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="firstDate">The earliest allowed date.</param>
    /// <param name="secondDate">The latest allowed date.</param>
    /// <returns>The constructed assertion.</returns>
    public static IAssertion<DateTime> IsBetween(this IAssertable<DateTime> assertion, DateTime firstDate, DateTime secondDate)
    {
        return assertion.Satisfies(
            predicate: x => x >= firstDate && x <= secondDate,
            errorFunc: _ => new RangeValidationError<DateTime>(lowerBound: firstDate, upperBound: secondDate));
    }

    /// <summary>
    ///     Check that a date is between two dates.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <param name="firstDate">The earliest allowed date.</param>
    /// <param name="secondDate">The latest allowed date.</param>
    /// <returns>The constructed assertion.</returns>
    /// <remarks>Always fails when the asserted <see cref="DateTime" /> is <c>null</c>.</remarks>
    public static IAssertion<DateTime?> IsBetween(this IAssertable<DateTime?> assertion, DateTime firstDate, DateTime secondDate)
    {
        return assertion.Satisfies(
            predicate: x => x >= firstDate && x <= secondDate,
            errorFunc: _ => new RangeValidationError<DateTime>(lowerBound: firstDate, upperBound: secondDate));
    }

    /// <summary>
    ///     Check that a datetime is specified as UTC.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <returns>The constructed assertion.</returns>
    public static IAssertion<DateTime> IsUtc(this IAssertable<DateTime> assertion)
    {
        return assertion.Satisfies(
            predicate: x => x.Kind == DateTimeKind.Utc,
            errorFunc: x => new TimeZoneValidationError(
                requiredTimeZone: TimeZoneInfo.Utc,
                actualTimeZone: x.Kind == DateTimeKind.Local ? TimeZoneInfo.Local : null));
    }

    /// <summary>
    ///     Check that a datetime is specified as UTC.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <returns>The constructed assertion.</returns>
    /// <remarks>Always fails when the asserted <see cref="DateTime" /> is <c>null</c>.</remarks>
    public static IAssertion<DateTime?> IsUtc(this IAssertable<DateTime?> assertion)
    {
        return assertion.Satisfies(
            predicate: x => x?.Kind == DateTimeKind.Utc,
            errorFunc: x => new TimeZoneValidationError(
                requiredTimeZone: TimeZoneInfo.Utc,
                actualTimeZone: x?.Kind == DateTimeKind.Local ? TimeZoneInfo.Local : null));
    }

    /// <summary>
    ///     Check that a datetime is specified as local time.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <returns>The constructed assertion.</returns>
    public static IAssertion<DateTime> IsLocalTime(this IAssertable<DateTime> assertion)
    {
        return assertion.Satisfies(
            predicate: x => x.Kind == DateTimeKind.Local,
            errorFunc: x => new TimeZoneValidationError(
                requiredTimeZone: TimeZoneInfo.Local,
                actualTimeZone: x.Kind == DateTimeKind.Utc ? TimeZoneInfo.Utc : null));
    }

    /// <summary>
    ///     Check that a datetime is specified as local time.
    /// </summary>
    /// <param name="assertion">The assertion to act on.</param>
    /// <returns>The constructed assertion.</returns>
    /// <remarks>Always fails when the asserted <see cref="DateTime" /> is <c>null</c>.</remarks>
    public static IAssertion<DateTime?> IsLocalTime(this IAssertable<DateTime?> assertion)
    {
        return assertion.Satisfies(
            predicate: x => x?.Kind == DateTimeKind.Local,
            errorFunc: x => new TimeZoneValidationError(
                requiredTimeZone: TimeZoneInfo.Local,
                actualTimeZone: x?.Kind == DateTimeKind.Utc ? TimeZoneInfo.Utc : null));
    }
}

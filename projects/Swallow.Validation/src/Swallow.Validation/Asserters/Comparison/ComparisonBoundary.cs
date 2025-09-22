namespace Swallow.Validation.Next.Asserters.Comparison;

/// <summary>
/// Represents a boundary against which a value may be checked.
/// </summary>
/// <param name="Value">The boundary value itself.</param>
/// <param name="IsInclusive">Whether <paramref name="Value"/> is an acceptable value or not.</param>
/// <typeparam name="T">Type of value that is being compared.</typeparam>
public readonly record struct ComparisonBoundary<T>(T Value, bool IsInclusive) where T : IComparable<T>;

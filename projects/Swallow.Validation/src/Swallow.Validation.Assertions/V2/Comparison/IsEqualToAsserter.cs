#nullable enable

namespace Swallow.Validation.V2.Comparison;

/// <summary>
/// An error signaling that a value was not equal to an expected value.
/// </summary>
/// <param name="expectedValue">The expected value.</param>
public sealed class NotEqualTo<T>(T expectedValue) : ValidationError
{
    /// <summary>
    /// The lower bound.
    /// </summary>
    public T ExpectedValue { get; } = expectedValue;

    /// <inheritdoc />
    public override string Message => $"be {ExpectedValue}";
}

/// <summary>
/// An asserter to check that a value is equal to an expected value; produces a
/// <see cref="NotEqualTo{T}"/> as validation error.
/// </summary>
/// <param name="expectedValue">The expected value.</param>
public sealed class IsEqualToAsserter<T>(T expectedValue) : IAsserter<T> where T : IEquatable<T>
{
    /// <inheritdoc />
    public bool IsValid(T value)
    {
        return expectedValue.Equals(value);
    }

    /// <inheritdoc />
    public ValidationError Error { get; } = new NotEqualTo<T>(expectedValue);
}

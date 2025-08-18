#nullable enable
using System.Diagnostics.CodeAnalysis;
using Swallow.Validation.Errors;

namespace Swallow.Validation.Assertions.Comparison;

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
    public override string Message => $"{PropertyName} is not equal to the expected value";
}

/// <summary>
/// An asserter to check that a value is equal to an expected value; produces a
/// <see cref="NotEqualTo{T}"/> as validation error.
/// </summary>
/// <param name="expectedValue">The expected value.</param>
public sealed class IsEqualToAsserter<T>(T expectedValue) : IAsserter<T> where T : IEquatable<T>
{
    /// <inheritdoc />
    public bool Check(INamedValueProvider<T> valueProvider, [NotNullWhen(false)] out ValidationError? error)
    {
        if (expectedValue.Equals(valueProvider.Value))
        {
            error = null;
            return true;
        }

        error = new NotEqualTo<T>(expectedValue);
        return false;
    }
}

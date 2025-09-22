namespace Swallow.Validation.V2.Utility;

/// <summary>
/// A <see cref="ValidationError"/> that singals that the inverse of another
/// <see cref="ValidationError"/> has happened.
/// </summary>
/// <param name="error">The underlying error.</param>
public sealed class InvertedError(ValidationError error) : ValidationError
{
    /// <summary>
    /// The underlying error.
    /// </summary>
    public ValidationError Error { get; } = error;

    /// <inheritdoc />
    public override string Message { get; } = $"not {error.Message}";
}


/// <summary>
/// An <see cref="IAsserter{T}"/> that inverts an inner asserter, i.e. reporting
/// success when the inner asserter fails and vice versa.
/// </summary>
/// <param name="asserter">The inner asserter.</param>
/// <typeparam name="T">Type of the checked value.</typeparam>
public sealed class InvertedAsserter<T>(IAsserter<T> asserter) : IAsserter<T>
{
    /// <inheritdoc />
    public bool IsValid(T value)
    {
        return !asserter.IsValid(value);
    }

    /// <inheritdoc />
    public ValidationError Error { get; } = new InvertedError(asserter.Error);
}

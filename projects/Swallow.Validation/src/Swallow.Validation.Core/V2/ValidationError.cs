namespace Swallow.Validation.V2;

/// <summary>
/// A validation error produced by an <see cref="IAsserter{T}"/>.
/// </summary>
public abstract class ValidationError
{
    /// <summary>
    /// A simple message describing the error; should be used when no other message is available.
    /// </summary>
    public abstract string Message { get; }
}

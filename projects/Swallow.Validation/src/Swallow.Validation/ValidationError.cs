namespace Swallow.Validation.Next;

/// <summary>
/// A validation error produced by an <see cref="IAsserter{T}"/>.
/// </summary>
public abstract class ValidationError
{
    /// <summary>
    /// A simple message describing the error; should only be used when no other message is available.
    /// </summary>
    public abstract string Message { get; }

    /// <inheritdoc />
    public override string ToString() => Message;
}

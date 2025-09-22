namespace Swallow.Validation.V2.Text;

/// <summary>
/// An error signaling that the asserted string is empty.
/// </summary>
public sealed class EmptyString : ValidationError
{
    /// <inheritdoc />
    public override string Message => "be not empty";
}

/// <summary>
/// An asserter to check whether a string is empty, produces <see cref="EmptyString"/> as
/// validation error.
/// </summary>
public sealed class IsNotEmptyAsserter : IAsserter<string>
{
    /// <inheritdoc />
    public bool IsValid(string value)
    {
        return value.Length > 0;
    }

    /// <inheritdoc />
    public ValidationError Error { get; } = new EmptyString();
}

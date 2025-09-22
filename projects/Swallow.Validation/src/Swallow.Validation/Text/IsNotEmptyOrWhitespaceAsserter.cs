namespace Swallow.Validation.V2.Text;

/// <summary>
/// An error signaling that the asserted string contains only whitespace characters.
/// </summary>
public sealed class OnlyWhitespaceString : ValidationError
{
    /// <inheritdoc />
    public override string Message => "be not only whitespace";
}

/// <summary>
/// An asserter to check whether a string consists only of whitespace characters, produces
/// <see cref="OnlyWhitespaceString"/> as validation error.
/// </summary>
public sealed class IsNotEmptyOrWhitespaceAsserter : IAsserter<string>
{
    /// <inheritdoc />
    public bool IsValid(string value)
    {
        return value.Length > 0 && value.Any(static c => !char.IsWhiteSpace(c));
    }

    /// <inheritdoc />
    public ValidationError Error { get; } = new OnlyWhitespaceString();
}

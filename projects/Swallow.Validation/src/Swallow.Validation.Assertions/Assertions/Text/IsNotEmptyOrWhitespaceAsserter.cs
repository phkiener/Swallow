#nullable enable
using System.Diagnostics.CodeAnalysis;
using Swallow.Validation.Errors;

namespace Swallow.Validation.Assertions.Text;

/// <summary>
/// An error signalling that the asserted string contains only whitespace characters.
/// </summary>
public sealed class OnlyWhitespaceString : ValidationError
{
    /// <inheritdoc />
    public override string Message => $"{PropertyName} contains only whitespace characters";
}

/// <summary>
/// An asserter to check whether a string consists only of whitespace characters, produces
/// <see cref="OnlyWhitespaceString"/> as validation error.
/// </summary>
public sealed class IsNotEmptyOrWhitespaceAsserter : IAsserter<string>
{
    /// <inheritdoc />
    public bool Check(INamedValueProvider<string> valueProvider, [NotNullWhen(false)] out ValidationError? error)
    {
        if (valueProvider.Value.Length is 0 || valueProvider.Value.All(char.IsWhiteSpace))
        {
            error = new OnlyWhitespaceString();
            return false;
        }

        error = null;
        return true;
    }
}

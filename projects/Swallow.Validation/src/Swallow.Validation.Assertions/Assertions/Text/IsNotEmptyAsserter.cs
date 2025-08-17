#nullable enable
using System.Diagnostics.CodeAnalysis;
using Swallow.Validation.Errors;

namespace Swallow.Validation.Assertions.Text;

/// <summary>
/// An error signaling that the asserted string is empty.
/// </summary>
public sealed class EmptyString : ValidationError
{
    /// <inheritdoc />
    public override string Message => $"{PropertyName} is empty";
}

/// <summary>
/// An asserter to check whether a string is empty, produces <see cref="EmptyString"/> as
/// validation error.
/// </summary>
public sealed class IsNotEmptyAsserter : IAsserter<string>
{
    /// <inheritdoc />
    public bool Check(INamedValueProvider<string> valueProvider, [NotNullWhen(false)] out ValidationError? error)
    {
        if (valueProvider.Value.Length is 0)
        {
            error = new EmptyString();
            return false;
        }

        error = null;
        return true;
    }
}

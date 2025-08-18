#nullable enable
using System.Diagnostics.CodeAnalysis;
using Swallow.Validation.Errors;

namespace Swallow.Validation.Assertions.Text;

/// <summary>
/// An error signaling that the asserted value is null.
/// </summary>
public sealed class ValueIsNull : ValidationError
{
    /// <inheritdoc />
    public override string Message => $"{PropertyName} is null";
}

/// <summary>
/// An asserter to check whether a value is null, produces <see cref="ValueIsNull"/> as
/// validation error.
/// </summary>
public sealed class IsNotNullAsserter<T> : IAsserter<T?>
{
    /// <inheritdoc />
    public bool Check(INamedValueProvider<T?> valueProvider, [NotNullWhen(false)] out ValidationError? error)
    {
        if (valueProvider.Value is null)
        {
            error = new ValueIsNull();
            return false;
        }

        error = null;
        return true;
    }
}

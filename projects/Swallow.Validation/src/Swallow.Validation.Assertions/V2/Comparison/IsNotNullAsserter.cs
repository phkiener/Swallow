#nullable enable

namespace Swallow.Validation.V2.Comparison;

/// <summary>
/// An error signaling that the asserted value is null.
/// </summary>
public sealed class ValueIsNull : ValidationError
{
    /// <inheritdoc />
    public override string Message => "be not null";
}

/// <summary>
/// An asserter to check whether a value is null, produces <see cref="ValueIsNull"/> as
/// validation error.
/// </summary>
public sealed class IsNotNullAsserter<T> : IAsserter<T?>
{
    /// <inheritdoc />
    public bool IsValid(T? value)
    {
        return value is not null;
    }

    /// <inheritdoc />
    public ValidationError Error { get; } = new ValueIsNull();
}

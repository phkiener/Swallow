#nullable enable
using System.Diagnostics.CodeAnalysis;
using Swallow.Validation.Errors;

namespace Swallow.Validation.Assertions.Times;

/// <summary>
/// An error signaling that a <see cref="DateTime"/> has the wrong <see cref="DateTimeKind"/>.
/// </summary>
/// <param name="expected">The expected <see cref="DateTimeKind"/>.</param>
public sealed class WrongDateTimeKind(DateTimeKind expected) : ValidationError
{
    /// <summary>
    /// The expected date time kind.
    /// </summary>
    public DateTimeKind Expected { get; } = expected;

    /// <inheritdoc />
    public override string Message => $"{PropertyName} is not of kind {Expected}";
}

/// <summary>
/// An asserter to check that a <see cref="DateTime"/> has the specified <see cref="DateTimeKind"/>;
/// produces a <see cref="WrongDateTimeKind"/> as validation error.
/// </summary>
/// <param name="expected">The expected <see cref="DateTimeKind"/> to check for.</param>
public sealed class IsDateTimeKindAsserter(DateTimeKind expected) : IAsserter<DateTime>
{
    /// <inheritdoc />
    public bool Check(INamedValueProvider<DateTime> valueProvider, [NotNullWhen(false)] out ValidationError? error)
    {
        if (valueProvider.Value.Kind == expected)
        {
            error = null;
            return true;
        }

        error = new WrongDateTimeKind(expected);
        return false;
    }
}

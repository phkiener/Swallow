namespace Swallow.Validation.Next.Asserters.Times;

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
    public override string Message => $"have kind {Expected}";
}

/// <summary>
/// An asserter to check that a <see cref="DateTime"/> has the specified <see cref="DateTimeKind"/>;
/// produces a <see cref="WrongDateTimeKind"/> as validation error.
/// </summary>
/// <param name="expected">The expected <see cref="DateTimeKind"/> to check for.</param>
public sealed class IsDateTimeKindAsserter(DateTimeKind expected) : IAsserter<DateTime>
{
    /// <inheritdoc />
    public bool IsValid(DateTime value)
    {
        return value.Kind == expected;
    }

    /// <inheritdoc />
    public ValidationError Error { get; } = new WrongDateTimeKind(expected);
}

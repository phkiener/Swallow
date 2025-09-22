using Swallow.Validation.Next.Asserters.Times;

namespace Swallow.Validation.Next.Asserters;

partial class Satisfies
{
    /// <summary>
    /// Returns an <see cref="IsDateTimeKindAsserter"/> that checks whether a given <see cref="DateTime"/> has the
    /// expected <see cref="DateTimeKind"/>.
    /// </summary>
    /// <param name="kind">The <see cref="DateTimeKind"/> to check against.</param>
    public static IAsserter<DateTime> HasKind(DateTimeKind kind) => new IsDateTimeKindAsserter(kind);
}

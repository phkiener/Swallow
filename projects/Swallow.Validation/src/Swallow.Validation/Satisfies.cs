using Swallow.Validation.Next.Asserters.Text;

namespace Swallow.Validation.Next;

/// <summary>
/// Convenience helpers to produce a plethora of different <see cref="IAsserter{T}"/>s.
/// </summary>
public static class Satisfies
{
    /// <summary>
    /// Returns an <see cref="IsNotEmptyAsserter"/> that checks whether a string is empty.
    /// </summary>
    public static IAsserter<string> NotEmpty { get; } = new IsNotEmptyAsserter();
}

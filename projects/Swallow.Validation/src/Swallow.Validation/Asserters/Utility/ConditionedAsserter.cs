using Swallow.Validation.Next.Internal;

namespace Swallow.Validation.Next.Asserters.Utility;

/// <summary>
/// An <see cref="IAsserter{T}"/> that delegates asserting to an inner asserter,
/// but only if a condition is met. Otherwise, it'll simply report success.
/// </summary>
/// <typeparam name="T">The type of asserted value.</typeparam>
public sealed class ConditionedAsserter<T> : IAsserter<T>
{
    private readonly IAsserter<T> asserter;
    private readonly ValueProvider<bool> doAssert;

    internal ConditionedAsserter(IAsserter<T> asserter, ValueProvider<bool> doAssert)
    {
        this.asserter = asserter;
        this.doAssert = doAssert;
    }

    /// <inheritdoc />
    public bool IsValid(T value)
    {
        return !doAssert.Value || asserter.IsValid(value);
    }

    /// <inheritdoc />
    public ValidationError Error => asserter.Error;
}

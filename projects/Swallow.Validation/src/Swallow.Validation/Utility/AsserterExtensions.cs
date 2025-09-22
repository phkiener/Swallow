namespace Swallow.Validation.V2.Utility;

/// <summary>
/// Extensions for working with <see cref="IAsserter{T}"/>s.
/// </summary>
public static class AsserterExtensions
{
    /// <summary>
    /// Turn a plain asserter into a <see cref="ConditionedAsserter{T}"/> that only
    /// asserts the value when <paramref name="condition"/> is <c>true</c>.
    /// </summary>
    /// <param name="asserter">The asserter to use.</param>
    /// <param name="condition">Whether the asserter should be executed.</param>
    /// <typeparam name="T">Type of value being asserted.</typeparam>
    /// <returns>The conditioned asserter to use.</returns>
    /// <seealso cref="Unless{T}(IAsserter{T}, bool)"/>
    public static IAsserter<T> When<T>(this IAsserter<T> asserter, bool condition)
    {
        return new ConditionedAsserter<T>(asserter, ValueProvider.Capture(condition));
    }

    /// <summary>
    /// Turn a plain asserter into a <see cref="ConditionedAsserter{T}"/> that only
    /// asserts the value when <paramref name="condition"/> retuns <c>true</c>.
    /// </summary>
    /// <param name="asserter">The asserter to use.</param>
    /// <param name="condition">Whether the asserter should be executed.</param>
    /// <typeparam name="T">Type of value being asserted.</typeparam>
    /// <returns>The conditioned asserter to use.</returns>
    /// <seealso cref="Unless{T}(IAsserter{T}, Func{bool})"/>
    public static IAsserter<T> When<T>(this IAsserter<T> asserter, Func<bool> condition)
    {
        return new ConditionedAsserter<T>(asserter, ValueProvider.Function(condition));
    }

    /// <summary>
    /// Turn a plain asserter into a <see cref="ConditionedAsserter{T}"/> that only
    /// asserts the value when <paramref name="condition"/> is <c>false</c>.
    /// </summary>
    /// <param name="asserter">The asserter to use.</param>
    /// <param name="condition">Whether the asserter should be skipped.</param>
    /// <typeparam name="T">Type of value being asserted.</typeparam>
    /// <returns>The conditioned asserter to use.</returns>
    /// <seealso cref="When{T}(IAsserter{T}, bool)"/>
    public static IAsserter<T> Unless<T>(this IAsserter<T> asserter, bool condition)
    {
        return new ConditionedAsserter<T>(asserter, ValueProvider.Capture(!condition));
    }

    /// <summary>
    /// Turn a plain asserter into a <see cref="ConditionedAsserter{T}"/> that only
    /// asserts the value when <paramref name="condition"/> retuns <c>false</c>.
    /// </summary>
    /// <param name="asserter">The asserter to use.</param>
    /// <param name="condition">Whether the asserter should be skipped.</param>
    /// <typeparam name="T">Type of value being asserted.</typeparam>
    /// <returns>The conditioned asserter to use.</returns>
    /// <seealso cref="When{T}(IAsserter{T}, Func{bool})"/>
    public static IAsserter<T> Unless<T>(this IAsserter<T> asserter, Func<bool> condition)
    {
        return new ConditionedAsserter<T>(asserter, ValueProvider.Function(() => !condition()));
    }
}

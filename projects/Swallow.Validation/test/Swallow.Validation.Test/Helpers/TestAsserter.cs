using Swallow.Validation.Next.Asserters.Text;

namespace Swallow.Validation.Next.Helpers;

internal sealed class TestAsserter<T>(bool fail, Action? onAssert) : IAsserter<T>
{
    public bool IsValid(T value)
    {
        onAssert?.Invoke();
        return !fail;
    }

    public ValidationError Error { get; } = new EmptyString();

    public static TestAsserter<T> Fail(Action? onAssert = null) => new(fail: true, onAssert);
    public static TestAsserter<T> Succeed(Action? onAssert = null) => new(fail: false, onAssert);
}

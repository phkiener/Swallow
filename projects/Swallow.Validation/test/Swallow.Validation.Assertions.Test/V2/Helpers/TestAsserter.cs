using Swallow.Validation.V2.Text;

namespace Swallow.Validation.V2;

internal sealed class TestAsserter<T>(bool fail, Action onAssert) : IAsserter<T>
{
    public bool IsValid(T value)
    {
        onAssert.Invoke();
        return !fail;
    }

    public ValidationError Error { get; } = new EmptyString();

    public static TestAsserter<T> Fail(Action onAssert) => new(fail: true, onAssert);
    public static TestAsserter<T> Succeed(Action onAssert) => new(fail: false, onAssert);
}

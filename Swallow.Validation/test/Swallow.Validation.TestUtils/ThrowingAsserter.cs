namespace Swallow.Validation.TestUtils;

using System;
using Errors;

public class ThrowingAsserter<T> : IAsserter<T>
{
    public Exception ThrownException { get; } = new InvalidOperationException("KERNEL PANIC");

    public bool Check(INamedValueProvider<T> valueProvider, out ValidationError error)
    {
        throw ThrownException;
    }
}

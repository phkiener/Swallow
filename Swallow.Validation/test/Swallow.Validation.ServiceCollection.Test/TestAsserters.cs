namespace Swallow.Validation.ServiceCollection;

using Errors;

internal abstract record TestAsserterBase<T> : IAsserter<T>
{
    public bool Check(INamedValueProvider<T> valueProvider, out ValidationError error)
    {
        error = null;

        return true;
    }
}

internal sealed record AnotherStringAsserter : TestAsserterBase<string>;
internal sealed record AsserterWithGeneric<T> : TestAsserterBase<T>;
internal sealed record IntAsserter : TestAsserterBase<int>;
internal sealed record ObjectAsserter : TestAsserterBase<object>;
internal sealed record StringAsserter : TestAsserterBase<string>;

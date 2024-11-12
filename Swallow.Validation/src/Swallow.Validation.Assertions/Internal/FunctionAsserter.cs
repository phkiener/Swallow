namespace Swallow.Validation.Internal;

using System;
using Errors;

internal sealed class FunctionAsserter<TValue> : IAsserter<TValue>
{
    private readonly Func<TValue, ValidationError> errorFunc;
    private readonly Func<TValue, bool> predicate;

    public FunctionAsserter(Func<TValue, bool> predicate, Func<TValue, ValidationError> errorFunc)
    {
        this.predicate = predicate;
        this.errorFunc = errorFunc;
    }

    public bool Check(INamedValueProvider<TValue> valueProvider, out ValidationError error)
    {
        var value = valueProvider.Value;
        var success = predicate(value);
        error = success ? null : errorFunc(value);

        return success;
    }
}

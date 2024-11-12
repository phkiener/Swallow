namespace Swallow.Validation.TestUtils;

using System.Linq;
using Errors;

public static class TestAsserter
{
    public static TestAsserter<TValue> Succeeding<TValue>()
    {
        return new() { ShouldSucceed = true };
    }

    public static TestAsserter<TValue> Failing<TValue>()
    {
        return new() { ShouldSucceed = false };
    }
}

public sealed class TestAsserter<T> : IAsserter<T>
{
    public int TimesCalled { get; private set; }
    public bool ShouldSucceed { get; init; } = true;
    public ValidationError GeneratedError { get; } = new TestValidationError();
    public T LastCheckedValue { get; private set; }

    public bool Check(INamedValueProvider<T> valueProvider, out ValidationError error)
    {
        TimesCalled += 1;
        LastCheckedValue = valueProvider.Value;
        var result = Validator.Check().That(valueProvider).Satisfies(predicate: _ => ShouldSucceed, errorFunc: _ => GeneratedError).Result();
        error = result.Errors.FirstOrDefault();

        return result.IsSuccess;
    }
}

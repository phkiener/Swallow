namespace Swallow.Validation.Internal;

using System;

internal class EvaluatedValue<T> : IValueProvider<T>
{
    private readonly Func<T> function;

    public EvaluatedValue(Func<T> function)
    {
        this.function = function;
    }

    public T Value => function.Invoke();
}

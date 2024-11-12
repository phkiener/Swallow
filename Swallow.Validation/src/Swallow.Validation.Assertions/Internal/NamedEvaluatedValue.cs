namespace Swallow.Validation.Internal;

using System;

internal sealed class NamedEvaluatedValue<T> : EvaluatedValue<T>, INamedValueProvider<T>
{
    public NamedEvaluatedValue(Func<T> function, string name) : base(function)
    {
        Name = name;
    }

    public string Name { get; }
}

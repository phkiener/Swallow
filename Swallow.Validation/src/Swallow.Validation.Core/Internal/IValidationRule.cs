namespace Swallow.Validation.Internal;

using System;
using Errors;

internal interface IValidationRule
{
    public string PropertyName { get; }
    public string FormattedValue { get; }

    public ValidationError Evaluate();
}

internal class ValidationRule<T> : IValidationRule
{
    private readonly INamedValueProvider<T> assertedValue;
    private readonly IAsserter<T> asserter;
    private readonly Func<T, string> formatter;

    public ValidationRule(INamedValueProvider<T> assertedValue, Func<T, string> formatter, IAsserter<T> asserter)
    {
        this.assertedValue = assertedValue;
        this.formatter = formatter;
        this.asserter = asserter;
    }

    public string PropertyName => assertedValue.Name;
    public string FormattedValue => formatter(assertedValue.Value);

    public virtual ValidationError Evaluate()
    {
        var result = asserter.Check(valueProvider: assertedValue, error: out var error);

        return result is false ? error : null;
    }
}

internal sealed class ValidationRuleWithCondition<T> : ValidationRule<T>
{
    private readonly IValueProvider<bool> condition;

    public ValidationRuleWithCondition(
        INamedValueProvider<T> assertedValue,
        Func<T, string> formatter,
        IAsserter<T> asserter,
        IValueProvider<bool> condition) : base(assertedValue: assertedValue, formatter: formatter, asserter: asserter)
    {
        this.condition = condition;
    }

    public override ValidationError Evaluate()
    {
        return condition.Value ? base.Evaluate() : null;
    }
}

internal sealed class ValidationRuleWithInvertedCondition<T> : ValidationRule<T>
{
    private readonly IValueProvider<bool> condition;

    public ValidationRuleWithInvertedCondition(
        INamedValueProvider<T> assertedValue,
        Func<T, string> formatter,
        IAsserter<T> asserter,
        IValueProvider<bool> condition) : base(assertedValue: assertedValue, formatter: formatter, asserter: asserter)
    {
        this.condition = condition;
    }

    public override ValidationError Evaluate()
    {
        return condition.Value is false ? base.Evaluate() : null;
    }
}

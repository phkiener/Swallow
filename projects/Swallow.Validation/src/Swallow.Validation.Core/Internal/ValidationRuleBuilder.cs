namespace Swallow.Validation.Internal;

using System;

internal sealed class ValidationRuleBuilder<T> : IUnformattedAssertable<T>, IAssertion<T>, IConditionedAssertion<T>
{
    private readonly INamedValueProvider<T> assertedValue;
    private readonly Func<IValidationRule, IValidation> finalizer;
    private IValueProvider<bool> condition;
    private bool conditionIsInverted;
    private IAsserter<T> currentAsserter;
    private Func<T, string> formatter = s => s?.ToString() ?? "null";

    public ValidationRuleBuilder(Func<IValidationRule, IValidation> finalizer, INamedValueProvider<T> assertedValue)
    {
        this.finalizer = finalizer;
        this.assertedValue = assertedValue;
    }

    public IUnformattedAssertable<TNext> That<TNext>(INamedValueProvider<TNext> namedValue)
    {
        CompleteRule();

        return new ValidationRuleBuilder<TNext>(finalizer: finalizer, assertedValue: namedValue);
    }

    public ValidationResult Result()
    {
        return CompleteRule().Result();
    }

    public IConditionedAssertion<T> When(IValueProvider<bool> valueProvider)
    {
        condition = valueProvider;

        return this;
    }

    public IConditionedAssertion<T> Unless(IValueProvider<bool> valueProvider)
    {
        condition = valueProvider;
        conditionIsInverted = true;

        return this;
    }

    public IAssertion<T> Satisfies(IAsserter<T> asserter)
    {
        if (currentAsserter is not null)
        {
            CompleteRule();

            return new ValidationRuleBuilder<T>(finalizer: finalizer, assertedValue: assertedValue).ShownAs(formatter).Satisfies(asserter);
        }

        currentAsserter = asserter;

        return this;
    }

    public IAssertable<T> ShownAs(Func<T, string> displayFunction)
    {
        formatter = displayFunction;

        return this;
    }

    private IValidation CompleteRule()
    {
        var builtRule = (conditionIsInverted, condition) switch
        {
            (_, null) => new ValidationRule<T>(assertedValue: assertedValue, formatter: formatter, asserter: currentAsserter!),
            (false, _) => new ValidationRuleWithCondition<T>(
                assertedValue: assertedValue,
                formatter: formatter,
                asserter: currentAsserter!,
                condition: condition),
            (true, _) => new ValidationRuleWithInvertedCondition<T>(
                assertedValue: assertedValue,
                formatter: formatter,
                asserter: currentAsserter!,
                condition: condition)
        };

        return finalizer(builtRule);
    }
}

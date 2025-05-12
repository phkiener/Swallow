namespace Swallow.Validation.Internal;

using System;
using System.Collections.Generic;
using System.Linq;
using Errors;

internal abstract class AbstractValidator : IValidation
{
    private readonly IRuleEvaluator ruleEvaluator;
    private readonly IRuleSkippingStrategy ruleSkippingStrategy;

    protected AbstractValidator(IRuleEvaluator ruleEvaluator, IRuleSkippingStrategy ruleSkippingStrategy)
    {
        this.ruleEvaluator = ruleEvaluator;
        this.ruleSkippingStrategy = ruleSkippingStrategy;
    }

    public IUnformattedAssertable<T> That<T>(INamedValueProvider<T> namedValue)
    {
        return new ValidationRuleBuilder<T>(finalizer: Accept, assertedValue: namedValue);
    }

    public ValidationResult Result()
    {
        var validationErrors = GetValidationErrors();

        return new(validationErrors.ToList());
    }

    protected abstract IValidation Accept(IValidationRule validationRule);

    protected abstract IEnumerable<ValidationError> GetValidationErrors();

    protected bool DoEvaluateRule(IValidationRule rule, IEnumerable<ValidationError> previousErrors)
    {
        return ruleSkippingStrategy.SkipValidationRule(rule: rule, errors: previousErrors) is false;
    }

    protected void EvaluateRule(IValidationRule rule, Action<ValidationError> onError)
    {
        var error = ruleEvaluator.Evaluate(rule);
        if (error is not null)
        {
            onError(error);
        }
    }
}

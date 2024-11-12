namespace Swallow.Validation.Internal;

using System;
using Errors;

internal interface IRuleEvaluator
{
    public ValidationError Evaluate(IValidationRule rule);
}

internal sealed class RuleEvaluator : IRuleEvaluator
{
    public ValidationError Evaluate(IValidationRule rule)
    {
        var error = rule.Evaluate();
        if (error is not null)
        {
            error.PropertyName = rule.PropertyName;
            error.ActualValue = rule.FormattedValue;
        }

        return error;
    }
}

internal sealed class ExceptionCatcher : IRuleEvaluator
{
    private readonly IRuleEvaluator inner;

    public ExceptionCatcher(IRuleEvaluator inner)
    {
        this.inner = inner;
    }

    public ValidationError Evaluate(IValidationRule rule)
    {
        try
        {
            return inner.Evaluate(rule);
        }
        catch (Exception e)
        {
            return new ExceptionValidationError(e) { PropertyName = rule.PropertyName, ActualValue = rule.FormattedValue };
        }
    }
}

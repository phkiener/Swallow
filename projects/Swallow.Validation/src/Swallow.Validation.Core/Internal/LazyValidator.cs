namespace Swallow.Validation.Internal;

using System.Collections.Generic;
using Errors;

internal sealed class LazyValidator : AbstractValidator
{
    private readonly Queue<IValidationRule> validationRules;

    public LazyValidator(IRuleEvaluator ruleEvaluator, IRuleSkippingStrategy ruleSkippingStrategy) : base(
        ruleEvaluator: ruleEvaluator,
        ruleSkippingStrategy: ruleSkippingStrategy)
    {
        validationRules = new();
    }

    protected override IValidation Accept(IValidationRule rule)
    {
        validationRules.Enqueue(rule);

        return this;
    }

    protected override IReadOnlyList<ValidationError> GetValidationErrors()
    {
        var validationErrors = new List<ValidationError>();
        while (validationRules.TryDequeue(out var rule))
        {
            if (DoEvaluateRule(rule: rule, previousErrors: validationErrors))
            {
                EvaluateRule(rule: rule, onError: validationErrors.Add);
            }
        }

        return validationErrors;
    }
}

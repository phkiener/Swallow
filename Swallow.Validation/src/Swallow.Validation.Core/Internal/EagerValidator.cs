namespace Swallow.Validation.Internal;

using System.Collections.Generic;
using Errors;

internal sealed class EagerValidator : AbstractValidator
{
    private readonly List<ValidationError> validationErrors;

    public EagerValidator(IRuleEvaluator ruleEvaluator, IRuleSkippingStrategy ruleSkippingStrategy) : base(
        ruleEvaluator: ruleEvaluator,
        ruleSkippingStrategy: ruleSkippingStrategy)
    {
        validationErrors = new();
    }

    protected override IValidation Accept(IValidationRule rule)
    {
        if (DoEvaluateRule(rule: rule, previousErrors: validationErrors))
        {
            EvaluateRule(rule: rule, onError: validationErrors.Add);
        }

        return this;
    }

    protected override IEnumerable<ValidationError> GetValidationErrors()
    {
        return validationErrors;
    }
}

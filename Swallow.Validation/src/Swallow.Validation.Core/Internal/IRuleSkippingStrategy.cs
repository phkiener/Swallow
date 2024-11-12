namespace Swallow.Validation.Internal;

using System.Collections.Generic;
using System.Linq;
using Errors;

internal interface IRuleSkippingStrategy
{
    public bool SkipValidationRule(IValidationRule rule, IEnumerable<ValidationError> errors);
}

internal sealed class EvaluateAllRules : IRuleSkippingStrategy
{
    public bool SkipValidationRule(IValidationRule rule, IEnumerable<ValidationError> errors)
    {
        return false;
    }
}

internal sealed class SkipRuleOnErrorsForSameProperty : IRuleSkippingStrategy
{
    public bool SkipValidationRule(IValidationRule rule, IEnumerable<ValidationError> errors)
    {
        return errors.Any(e => e.PropertyName == rule.PropertyName);
    }
}

internal sealed class SkipRuleOnAnyError : IRuleSkippingStrategy
{
    public bool SkipValidationRule(IValidationRule rule, IEnumerable<ValidationError> errors)
    {
        return errors.Any();
    }
}

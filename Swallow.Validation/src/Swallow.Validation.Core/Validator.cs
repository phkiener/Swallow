namespace Swallow.Validation;

using System;
using Internal;

/// <summary>
///     A validator which handles validation rules by asserting different values.
/// </summary>
public static class Validator
{
    /// <summary>
    ///     Begin a new validation with default configuration.
    /// </summary>
    /// <returns>The validation context.</returns>
    public static IValidation Check()
    {
        return Check(ValidatorConfiguration.Default);
    }

    /// <summary>
    ///     Begin a new validation with a given configuration.
    /// </summary>
    /// <param name="configuration">The validator configuration.</param>
    /// <returns>The validation context.</returns>
    public static IValidation Check(ValidatorConfiguration configuration)
    {
        var ruleSkippingStrategy = CreateRuleSkippingStrategyFor(configuration.FailureHandling);
        var evaluator = CreateRuleEvaluatorFor(configuration.CatchExceptions);

        return configuration.ValidateLazily
            ? new LazyValidator(ruleEvaluator: evaluator, ruleSkippingStrategy: ruleSkippingStrategy)
            : new EagerValidator(ruleEvaluator: evaluator, ruleSkippingStrategy: ruleSkippingStrategy);
    }

    private static IRuleSkippingStrategy CreateRuleSkippingStrategyFor(FailureHandling failureHandling)
    {
        return failureHandling switch
        {
            FailureHandling.KeepGoing => new EvaluateAllRules(),
            FailureHandling.SkipAllRemaining => new SkipRuleOnAnyError(),
            FailureHandling.SkipAllForProperty => new SkipRuleOnErrorsForSameProperty(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static IRuleEvaluator CreateRuleEvaluatorFor(bool catchExceptions)
    {
        return catchExceptions ? new ExceptionCatcher(new RuleEvaluator()) : new RuleEvaluator();
    }
}

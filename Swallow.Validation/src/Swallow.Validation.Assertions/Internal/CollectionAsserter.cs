namespace Swallow.Validation.Internal;

using System;
using System.Collections.Generic;
using Errors;

internal sealed class CollectionAsserter<T> : IAsserter<IEnumerable<T>>
{
    private readonly Func<IUnformattedAssertable<T>, IValidation> elementAsserter;

    internal CollectionAsserter(Func<IUnformattedAssertable<T>, IValidation> elementAsserter)
    {
        this.elementAsserter = elementAsserter;
    }

    public bool Check(INamedValueProvider<IEnumerable<T>> valueProvider, out ValidationError error)
    {
        var config = new ValidatorConfiguration
        {
            ValidateLazily = false, FailureHandling = FailureHandling.SkipAllForProperty, CatchExceptions = true
        };

        var validator = Validator.Check(config);
        var index = 0;
        foreach (var element in valueProvider.Value)
        {
            var assertion = validator.That(value: element, name: $"{valueProvider.Name}[{index++}]");
            validator = elementAsserter(assertion);
        }

        var validationResult = validator.Result();
        error = validationResult.IsSuccess ? null : new EntityValidationError(validationResult.Errors);

        return validationResult.IsSuccess;
    }
}

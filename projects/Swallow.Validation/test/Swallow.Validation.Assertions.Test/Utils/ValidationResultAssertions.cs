namespace Swallow.Validation.Utils;

using System.Linq;
using Errors;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

internal sealed class ValidationResultAssertions : ObjectAssertions<ValidationResult, ValidationResultAssertions>
{
    public ValidationResultAssertions(ValidationResult value) : base(value) { }

    public void BeSuccess()
    {
        Execute.Assertion.ForCondition(Subject.IsSuccess)
            .WithDefaultIdentifier(Identifier)
            .FailWith("Expected {context} to be Success, but found Error.");
    }

    public AndWhichConstraint<ValidationResult, TError> HaveError<TError>() where TError : ValidationError
    {
        var matchingError = Subject.Errors.OfType<TError>().FirstOrDefault();
        Execute.Assertion.ForCondition(matchingError != null)
            .WithDefaultIdentifier(Identifier)
            .FailWith(message: "Expected {context} to contain an error of type {0}, but found none that match.", typeof(TError));

        return new(parentConstraint: Subject, matchedConstraint: matchingError!);
    }
}

internal static class ValidationResultAssertionExtensions
{
    public static ValidationResultAssertions Should(this ValidationResult validationResult)
    {
        return new(validationResult);
    }
}

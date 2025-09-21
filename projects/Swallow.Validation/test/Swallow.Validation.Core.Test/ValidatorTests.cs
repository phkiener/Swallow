namespace Swallow.Validation;

using System;
using System.Linq;
using Errors;
using NUnit.Framework;
using TestUtils;

[TestFixture]
public sealed class ValidatorTests
{
    [Test]
    public void ReportExceptionsAsExceptionValidationError_WhenCatchExceptionsIsEnabled()
    {
        var asserter = new ThrowingAsserter<int>();

        var result = Validator.Check(new() { CatchExceptions = true })
            .That(TestValue.Of(5)).Satisfies(asserter)
            .Result();

        var error = result.Errors.Single() as ExceptionValidationError;
        Assert.That(error?.Exception, Is.EqualTo(asserter.ThrownException));
    }

    [Test]
    public void ThrowExceptionFromAsserter_WhenCatchExceptionsIsDisabled()
    {
        var asserter = new ThrowingAsserter<int>();

        var act = () => Validator.Check(new() { CatchExceptions = false })
            .That(TestValue.Of(5)).Satisfies(asserter)
            .Result();

        var exception = Assert.Throws<InvalidOperationException>(() => act());
        Assert.That(exception!.Message, Is.EqualTo(asserter.ThrownException.Message));
    }

    [Test]
    public void EvaluateAssertionAsSoonAsPossible_WhenValidateLazilyIsDisabled()
    {
        var asserter = TestAsserter.Succeeding<int>();
        var followingAsserter = TestAsserter.Succeeding<int>();

        Validator.Check(new() { ValidateLazily = false })
            .That(TestValue.Of(5)).Satisfies(asserter)
            .Satisfies(followingAsserter);

        Assert.That(asserter.TimesCalled, Is.EqualTo(1));
    }

    [Test]
    public void NotEvaluateAssertion_WhenValidateLazilyIsEnabledAndResultIsNotCalled()
    {
        var asserter = TestAsserter.Succeeding<int>();
        var followingAsserter = TestAsserter.Succeeding<int>();

        Validator.Check(new() { ValidateLazily = true })
            .That(TestValue.Of(5)).Satisfies(asserter).Satisfies(followingAsserter);

        Assert.That(asserter.TimesCalled, Is.EqualTo(0));
    }

    [Test]
    public void EvaluateAssertion_WhenValidateLazilyIsEnabledAndResultIsCalled()
    {
        var asserter = TestAsserter.Failing<int>();
        var followingAsserter = TestAsserter.Succeeding<int>();

        Validator.Check(new() { ValidateLazily = true })
            .That(TestValue.Of(5)).Satisfies(asserter).Satisfies(followingAsserter)
            .Result();

        Assert.That(asserter.TimesCalled, Is.EqualTo(1));
    }

    [Test]
    public void ContinueAssertingOnErrors_WhenFailureHandlingIsKeepGoing()
    {
        var failingAsserter = TestAsserter.Failing<int>();
        var succeedingAsserter = TestAsserter.Succeeding<int>();

        Validator.Check(new() { FailureHandling = FailureHandling.KeepGoing })
            .That(TestValue.Of(5)).Satisfies(failingAsserter)
            .That(TestValue.Of(6)).Satisfies(succeedingAsserter)
            .Result();

        Assert.That(failingAsserter.TimesCalled, Is.EqualTo(1));
        Assert.That(succeedingAsserter.TimesCalled, Is.EqualTo(1));
    }

    [Test]
    public void SkipAssertionsForPropertiesWithSameNameOnErrors_WhenFailureHandlingIsSkipAllForProperty()
    {
        var failingAsserter = TestAsserter.Failing<int>();
        var succeedingAsserter = TestAsserter.Succeeding<int>();
        var succeedingStringAsserter = TestAsserter.Succeeding<string>();

        Validator.Check(new() { FailureHandling = FailureHandling.SkipAllForProperty })
            .That(TestValue.Of(value: 5, name: "five")).Satisfies(failingAsserter)
            .That(TestValue.Of(value: 5, name: "five")).Satisfies(succeedingAsserter)
            .That(TestValue.Of(value: "6", name: "six")).Satisfies(succeedingStringAsserter)
            .Result();

        Assert.That(failingAsserter.TimesCalled, Is.EqualTo(1));
        Assert.That(succeedingAsserter.TimesCalled, Is.EqualTo(0));
        Assert.That(succeedingStringAsserter.TimesCalled, Is.EqualTo(1));
    }

    [Test]
    public void SkipAllRemainingAssertionsOnErrors_WhenFailureHandlingIsSkipAllRemaining()
    {
        var failingAsserter = TestAsserter.Failing<int>();
        var succeedingAsserter = TestAsserter.Succeeding<int>();

        Validator.Check(new() { FailureHandling = FailureHandling.SkipAllRemaining })
            .That(TestValue.Of(5)).Satisfies(failingAsserter)
            .That(TestValue.Of(6)).Satisfies(succeedingAsserter)
            .Result();

        Assert.That(failingAsserter.TimesCalled, Is.EqualTo(1));
        Assert.That(succeedingAsserter.TimesCalled, Is.EqualTo(0));
    }

    [Test]
    public void ReturnSuccess_WhenNoAssertionsAreDefined()
    {
        var result = Validator.Check().Result();

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.IsError, Is.False);
        Assert.That(result.Errors, Is.Empty);
    }

    [Test]
    public void ReturnSuccess_WhenAllAssertionsSucceed()
    {
        var assertion = TestAsserter.Succeeding<bool>();

        var result = Validator.Check().That(TestValue.Of(true))
            .Satisfies(assertion)
            .Result();

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.IsError, Is.False);
        Assert.That(result.Errors, Is.Empty);
    }

    [Test]
    public void ReturnFailure_WhenAssertionFailed()
    {
        var assertion = TestAsserter.Failing<bool>();

        var valueProvider = TestValue.Of(true);
        var result = Validator.Check()
            .That(TestValue.Of(true)).Satisfies(assertion)
            .Result();

        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.IsError, Is.True);
        Assert.That(result.Errors, Has.Count.EqualTo(1));

        var validationError = result.Errors.Single();
        Assert.That(validationError.Message, Is.EqualTo(assertion.GeneratedError.Message));
        Assert.That(validationError.ActualValue, Is.EqualTo(valueProvider.Value.ToString()));
        Assert.That(validationError.PropertyName, Is.EqualTo(valueProvider.Name));
    }

    [Test]
    public void ReturnFailureWithOverriddenActualValue_WhenAssertableIsFormattedUsingShownAs()
    {
        var assertion = TestAsserter.Failing<bool>();

        const string displayedValue = "bool: true";
        var valueProvider = TestValue.Of(true);
        var result = Validator.Check()
            .That(valueProvider).ShownAs(_ => displayedValue).Satisfies(assertion)
            .Result();

        var validationError = result.Errors.Single();
        Assert.That(validationError.ActualValue, Is.EqualTo(displayedValue));
    }

    [Test]
    public void ReturnFailureWithNullAsActualValue_WhenAssertedValueIsNull()
    {
        var assertion = TestAsserter.Failing<string>();

        var valueProvider = TestValue.Of<string>(null);
        var result = Validator.Check()
            .That(valueProvider).Satisfies(assertion)
            .Result();

        var validationError = result.Errors.Single();
        Assert.That(validationError.ActualValue, Is.EqualTo("null"));
    }
}

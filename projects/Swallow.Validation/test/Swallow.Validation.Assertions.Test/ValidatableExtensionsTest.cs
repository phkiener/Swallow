namespace Swallow.Validation;

using System.Linq;
using Exceptions;
using NUnit.Framework;
using TestUtils;

[TestFixture]
public sealed class ValidatableExtensionsTest
{
    [Test]
    public void InvokeGivenAction_WhenValidationFails()
    {
        var actionCalled = false;

        Validator.Check()
            .That(() => 5).Satisfies(TestAsserter.Failing<int>())
            .Else(_ => actionCalled = true);

        Assert.That(actionCalled, Is.True);
    }

    [Test]
    public void NotInvokeGivenAction_WhenValidationSucceeds()
    {
        var actionCalled = false;

        Validator.Check().That(() => 5)
            .Satisfies(TestAsserter.Succeeding<int>())
            .Else(_ => actionCalled = true);

        Assert.That(actionCalled, Is.False);
    }

    [Test]
    public void ThrowValidationException_WhenValidationFailed()
    {
        var act = () => Validator.Check()
            .That(() => 5).Satisfies(TestAsserter.Failing<int>())
            .ElseThrow();

        var exception = Assert.Throws<ValidationException>(() => act());
        Assert.That(exception!.Errors.Single(), Is.InstanceOf<TestValidationError>());
    }

    [Test]
    public void NotThrowValidationException_WhenValidationSucceeds()
    {
        var act = () => Validator.Check()
            .That(() => 5).Satisfies(TestAsserter.Succeeding<int>())
            .ElseThrow();

        Assert.DoesNotThrow(() => act());
    }
    [Test]
    public void UseGivenNameForPropertyNameAndEvaluateFunctionForValue_WhenGivenFunction()
    {
        var asserter = TestAsserter.Failing<int>();
        var result = Validator.Check()
            .That(valueFunc: () => 5, name: "value").Satisfies(asserter)
            .Result();

        Assert.That(asserter.LastCheckedValue, Is.EqualTo(5));
        Assert.That(result.Errors.Single().PropertyName, Is.EqualTo("value"));
    }

    [Test]
    public void DeterminePropertyNameViaCallerArgumentExpressionAttribute_WhenGivenFunction()
    {
        var asserter = TestAsserter.Failing<int>();
        var result = Validator.Check().
            That(() => 5).Satisfies(asserter)
            .Result();

        Assert.That(asserter.LastCheckedValue, Is.EqualTo(5));
        Assert.That(result.Errors.Single().PropertyName, Is.EqualTo("5"));
    }

    [Test]
    public void UseGivenNameForPropertyNameAndGivenValueForValue_WhenGivenValue()
    {
        var asserter = TestAsserter.Failing<int>();
        var result = Validator.Check()
            .That(value: 5, name: "value").Satisfies(asserter)
            .Result();

        Assert.That(asserter.LastCheckedValue, Is.EqualTo(5));
        Assert.That(result.Errors.Single().PropertyName, Is.EqualTo("value"));
    }

    [Test]
    public void DeterminePropertyNameViaCallerArgumentExpressionAttribute_WhenGivenValue()
    {
        var asserter = TestAsserter.Failing<int>();
        var result = Validator.Check()
            .That(5).Satisfies(asserter)
            .Result();

        Assert.That(asserter.LastCheckedValue, Is.EqualTo(5));
        Assert.That(result.Errors.Single().PropertyName, Is.EqualTo("5"));
    }
    [Test]
    public void ReturnResultOfFunction_WhenValidationSucceeds()
    {
        var result = Validator.Check()
            .That(() => 5).Satisfies(TestAsserter.Succeeding<int>())
            .Then(() => "hello world");

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.EqualTo("hello world"));
    }

    [Test]
    public void ReturnValidationError_WhenValidationFails()
    {
        var result = Validator.Check()
            .That(() => 5).Satisfies(TestAsserter.Failing<int>())
            .Then(() => "hello world");

        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.IsError, Is.True);
        Assert.That(result.Errors, Has.Count.EqualTo(1));
    }
}

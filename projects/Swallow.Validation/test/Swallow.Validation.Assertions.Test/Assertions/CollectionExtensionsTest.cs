namespace Swallow.Validation.Assertions;

using System.Collections.Generic;
using System.Linq;
using Errors;
using NUnit.Framework;
using Utils;

[TestFixture]
public sealed class CollectionExtensionsTest
{
    [Test]
    public void HasAll_FailAssertion_WhenNoValuesSatisfyInnerAssertion()
    {
        var values = new[] { "1", "2", "3" };
        var result = AssertionTester.Assert<IEnumerable<string>>(value: values, assertion: v => v.HasAll(x => x.Matches("[a-zA-Z]+")));

        var typedError = result.Errors.OfType<EntityValidationError>().Single();

        Assert.That(typedError.ValidationErrors, Has.Count.EqualTo(3));
        Assert.That(typedError.ValidationErrors[0].PropertyName, Is.EqualTo("value[0]"));
        Assert.That(typedError.ValidationErrors[1].PropertyName, Is.EqualTo("value[1]"));
        Assert.That(typedError.ValidationErrors[2].PropertyName, Is.EqualTo("value[2]"));
    }

    [Test]
    public void HasAll_PassAssertion_WhenAllValuesSatisfyInnerAssertion()
    {
        var values = new[] { "alpha", "beta", "gamma" };
        var result = AssertionTester.Assert<IEnumerable<string>>(value: values, assertion: v => v.HasAll(x => x.Matches("[a-zA-Z]+")));

        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void HasAll_FailAssertion_WhenOneValueDoesNotSatisfyInnerAssertion()
    {
        var values = new[] { "alpha", "2", "gamma" };
        var result = AssertionTester.Assert<IEnumerable<string>>(value: values, assertion: v => v.HasAll(x => x.Matches("[a-zA-Z]+")));

        var typedError = result.Errors.OfType<EntityValidationError>().Single();
        Assert.That(typedError.ValidationErrors[0].PropertyName, Is.EqualTo("value[1]"));
    }

    [Test]
    public void HasAllValid_PassAssertion_WhenAllElementsAreValid()
    {
        var values = new[] { new TestValidatableEntity { ShouldSucceed = true }, new TestValidatableEntity { ShouldSucceed = true } };
        var result = AssertionTester.Assert<IEnumerable<TestValidatableEntity>>(value: values, assertion: v => v.HasAllValid());

        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void FailValidation_WhenOneElementIsNotValid()
    {
        var values = new[] { new TestValidatableEntity { ShouldSucceed = false }, new TestValidatableEntity { ShouldSucceed = true } };
        var result = AssertionTester.Assert<IEnumerable<TestValidatableEntity>>(value: values, assertion: v => v.HasAllValid());

        var typedError = result.Errors.OfType<EntityValidationError>().Single();
        var innerError = typedError.ValidationErrors.OfType<EntityValidationError>().Single();

        Assert.That(innerError.PropertyName, Is.EqualTo("value[0]"));
    }

    [Test]
    public void IsIn_PassAssertion_WhenValueIsInGivenSet()
    {
        var result = AssertionTester.Assert(value: "alpha", assertion: v => v.IsIn(["alpha", "beta", "gamma"]));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsIn_FailAssertion_WhenValueIsNotInGivenSet()
    {
        var result = AssertionTester.Assert(value: "delta", assertion: v => v.IsIn(["alpha", "beta", "gamma"]));

        var typedError = result.Errors.OfType<DisallowedValueValidationError<string>>().Single();
        Assert.That(typedError.ShouldNotMatch, Is.False);
    }

    [Test]
    public void IsNotEmpty_PassAssertion_WhenValueContainsAnElement()
    {
        var result = AssertionTester.Assert<IEnumerable<string>>(value: ["yo"], assertion: v => v.IsNotEmpty());
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsNotEmpty_FailAssertion_WhenValueIsEmpty()
    {
        var result = AssertionTester.Assert<IEnumerable<string>>(value: [], assertion: v => v.IsNotEmpty());
        Assert.That(result.Errors, Has.One.InstanceOf<EmptyCollectionValidationError>());
    }

    [Test]
    public void IsNotIn_FailAssertion_WhenValueIsInGivenSet()
    {
        var result = AssertionTester.Assert(value: "alpha", assertion: v => v.IsNotIn(["alpha", "beta", "gamma"]));

        var typedError = result.Errors.OfType<DisallowedValueValidationError<string>>().Single();
        Assert.That(typedError.ShouldNotMatch, Is.True);
    }

    [Test]
    public void IsNotIn_PassAssertion_WhenValueIsNotInGivenSet()
    {
        var result = AssertionTester.Assert(value: "delta", assertion: v => v.IsNotIn(["alpha", "beta", "gamma"]));
        Assert.That(result.IsSuccess, Is.True);
    }
}

namespace Swallow.Validation.Assertions;

using System.Linq;
using Errors;
using NUnit.Framework;
using Utils;

[TestFixture]
public class ObjectExtensionsTest
{
    [Test]
    public void IsType_PassAssertion_WhenObjectHasCorrectType()
    {
        var result = AssertionTester.Assert<object>(value: new Derived(), assertion: v => v.IsType(typeof(Derived)));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsType_FailAssertion_WhenObjectHasWrongType()
    {
        var result = AssertionTester.Assert<object>(value: new Derived(), assertion: v => v.IsType(typeof(IBase)));

        var typedError = result.Errors.OfType<TypeValidationError>().Single();
        Assert.That(typedError.ExpectedType, Is.EqualTo(typeof(IBase)));
        Assert.That(typedError.ActualType, Is.EqualTo(typeof(Derived)));
        Assert.That(typedError.Mode, Is.EqualTo(TypeValidationError.MatchingMode.SameType));
    }

    [Test]
    public void IsType_PassAssertion_WhenObjectHasCorrectTypeViaTypeParameter()
    {
        var result = AssertionTester.Assert<object>(value: new Derived(), assertion: v => v.IsType<Derived>());
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsType_FailAssertion_WhenObjectHasWrongTypeViaTypeParameter()
    {
        // Act
        var result = AssertionTester.Assert<object>(value: new Derived(), assertion: v => v.IsType<IBase>());

        var typedError = result.Errors.OfType<TypeValidationError>().Single();
        Assert.That(typedError.ExpectedType, Is.EqualTo(typeof(IBase)));
        Assert.That(typedError.ActualType, Is.EqualTo(typeof(Derived)));
        Assert.That(typedError.Mode, Is.EqualTo(TypeValidationError.MatchingMode.SameType));
    }

    [Test]
    public void IsAssignableTo_PassAssertion_WhenObjectHasSameType()
    {
        var result = AssertionTester.Assert<object>(value: new Derived(), assertion: v => v.IsAssignableTo(typeof(Derived)));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsAssignableTo_PassAssertion_WhenObjectIsAssignableToGivenType()
    {
        var result = AssertionTester.Assert<object>(value: new Derived(), assertion: v => v.IsAssignableTo(typeof(IBase)));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsAssignableTo_FailAssertion_WhenObjectIsNotAssignableToGivenType()
    {
        // Act
        var result = AssertionTester.Assert<object>(value: new Derived(), assertion: v => v.IsAssignableTo(typeof(IOtherBase)));

        var typedError = result.Errors.OfType<TypeValidationError>().Single();
        Assert.That(typedError.ExpectedType, Is.EqualTo(typeof(IOtherBase)));
        Assert.That(typedError.ActualType, Is.EqualTo(typeof(Derived)));
        Assert.That(typedError.Mode, Is.EqualTo(TypeValidationError.MatchingMode.AssignableTo));
    }

    [Test]
    public void IsAssignableTo_PassAssertion_WhenObjectHasSameTypeViaTypeParameter()
    {
        var result = AssertionTester.Assert<object>(value: new Derived(), assertion: v => v.IsAssignableTo<Derived>());
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsAssignableTo_PassAssertion_WhenObjectIsAssignableToGivenTypeViaTypeParameter()
    {
        var result = AssertionTester.Assert<object>(value: new Derived(), assertion: v => v.IsAssignableTo<IBase>());
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void IsAssignableTo_FailAssertion_WhenObjectIsNotAssignableToGivenTypeViaTypeParameter()
    {
        // Act
        var result = AssertionTester.Assert<object>(value: new Derived(), assertion: v => v.IsAssignableTo<IOtherBase>());

        var typedError = result.Errors.OfType<TypeValidationError>().Single();
        Assert.That(typedError.ExpectedType, Is.EqualTo(typeof(IOtherBase)));
        Assert.That(typedError.ActualType, Is.EqualTo(typeof(Derived)));
        Assert.That(typedError.Mode, Is.EqualTo(TypeValidationError.MatchingMode.AssignableTo));
    }

    private interface IBase;
    private class Derived : IBase;
    private interface IOtherBase;
}

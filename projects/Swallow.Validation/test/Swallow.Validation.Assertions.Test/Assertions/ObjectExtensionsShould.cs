namespace Swallow.Validation.Assertions;

using System.Linq;
using Errors;
using FluentAssertions;
using NUnit.Framework;
using Utils;

[TestFixture]
internal class ObjectExtensionsShould
{
    private interface IBase;
    private class Derived : IBase;
    private interface IOtherBase;

    public sealed class OnIsType : ObjectExtensionsShould
    {
        [Test]
        public void PassAssertion_WhenObjectHasCorrectType()
        {
            // Act
            var result = AssertionTester.Assert<object>(value: new Derived(), assertion: v => v.IsType(typeof(Derived)));

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenObjectHasWrongType()
        {
            // Act
            var result = AssertionTester.Assert<object>(value: new Derived(), assertion: v => v.IsType(typeof(IBase)));

            // Assert
            result.Should().HaveError<TypeValidationError>();
            var error = result.Errors.Single().As<TypeValidationError>();
            error.ExpectedType.Should().Be(typeof(IBase));
            error.ActualType.Should().Be(typeof(Derived));
            error.Mode.Should().Be(TypeValidationError.MatchingMode.SameType);
        }

        [Test]
        public void PassAssertion_WhenObjectHasCorrectTypeViaTypeParameter()
        {
            // Act
            var result = AssertionTester.Assert<object>(value: new Derived(), assertion: v => v.IsType<Derived>());

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenObjectHasWrongTypeViaTypeParameter()
        {
            // Act
            var result = AssertionTester.Assert<object>(value: new Derived(), assertion: v => v.IsType<IBase>());

            // Assert
            result.Should().HaveError<TypeValidationError>();
            var error = result.Errors.Single().As<TypeValidationError>();
            error.ExpectedType.Should().Be(typeof(IBase));
            error.ActualType.Should().Be(typeof(Derived));
            error.Mode.Should().Be(TypeValidationError.MatchingMode.SameType);
        }
    }

    public sealed class OnIsAssignableTo : ObjectExtensionsShould
    {
        [Test]
        public void PassAssertion_WhenObjectHasSameType()
        {
            // Act
            var result = AssertionTester.Assert<object>(value: new Derived(), assertion: v => v.IsAssignableTo(typeof(Derived)));

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void PassAssertion_WhenObjectIsAssignableToGivenType()
        {
            // Act
            var result = AssertionTester.Assert<object>(value: new Derived(), assertion: v => v.IsAssignableTo(typeof(IBase)));

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenObjectIsNotAssignableToGivenType()
        {
            // Act
            var result = AssertionTester.Assert<object>(value: new Derived(), assertion: v => v.IsAssignableTo(typeof(IOtherBase)));

            // Assert
            result.Should().HaveError<TypeValidationError>();
            var error = result.Errors.Single().As<TypeValidationError>();
            error.ExpectedType.Should().Be(typeof(IOtherBase));
            error.ActualType.Should().Be(typeof(Derived));
            error.Mode.Should().Be(TypeValidationError.MatchingMode.AssignableTo);
        }

        [Test]
        public void PassAssertion_WhenObjectHasSameTypeViaTypeParameter()
        {
            // Act
            var result = AssertionTester.Assert<object>(value: new Derived(), assertion: v => v.IsAssignableTo<Derived>());

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void PassAssertion_WhenObjectIsAssignableToGivenTypeViaTypeParameter()
        {
            // Act
            var result = AssertionTester.Assert<object>(value: new Derived(), assertion: v => v.IsAssignableTo<IBase>());

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenObjectIsNotAssignableToGivenTypeViaTypeParameter()
        {
            // Act
            var result = AssertionTester.Assert<object>(value: new Derived(), assertion: v => v.IsAssignableTo<IOtherBase>());

            // Assert
            result.Should().HaveError<TypeValidationError>();
            var error = result.Errors.Single().As<TypeValidationError>();
            error.ExpectedType.Should().Be(typeof(IOtherBase));
            error.ActualType.Should().Be(typeof(Derived));
            error.Mode.Should().Be(TypeValidationError.MatchingMode.AssignableTo);
        }
    }
}

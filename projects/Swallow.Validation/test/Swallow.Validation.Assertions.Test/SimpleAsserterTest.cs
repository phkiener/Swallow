namespace Swallow.Validation;

using Errors;
using NUnit.Framework;
using TestUtils;

[TestFixture]
public sealed class SimpleAsserterTest
{
    [Test]
    public void ReturnFalse_WhenAsserterFails()
    {
        var valueProvider = TestValue.Of(1337);
        var asserter = new DerivedSimpleAsserter { ShouldSucceed = false };

        var result = asserter.Check(valueProvider: valueProvider, error: out var error);

        Assert.That(result, Is.False);

        var typedError = error as EntityValidationError;
        Assert.That(typedError?.ValidationErrors, Has.Count.EqualTo(1));
        Assert.That(typedError?.ValidationErrors, Has.All.Matches<ValidationError>(v => v.PropertyName == valueProvider.Name));
    }

    [Test]
    public void Check_ValidateProducesSuccess_ReturnsTrueAndSetsErrorToNull()
    {
        var valueProvider = TestValue.Of(42);
        var asserter = new DerivedSimpleAsserter { ShouldSucceed = true };

        var result = asserter.Check(valueProvider: valueProvider, error: out var error);

        Assert.That(result, Is.True);
        Assert.That(error, Is.Null);
    }

    private sealed class DerivedSimpleAsserter : SimpleAsserter<int>
    {
        public bool ShouldSucceed { get; init; } = true;

        protected override ValidationResult Validate(int value)
        {
            return Validator.Check()
                .That(value: value, name: PropertyName)
                .Satisfies(predicate: _ => ShouldSucceed, errorFunc: _ => new TestValidationError())
                .Result();
        }
    }
}

namespace Swallow.Validation.Validatable;

using Errors;
using NUnit.Framework;
using TestUtils;
using Utils;

[TestFixture]
public sealed class ValidatableEntityAsserterTest
{
    [Test]
    public void ReturnFalse_WhenCheckedValueIsNotValid()
    {
        var entity = new TestValidatableEntity { ShouldSucceed = false };
        var asserter = new ValidatableEntityAsserter<TestValidatableEntity>();
        var valueProvider = TestValue.Of(entity);

        var result = asserter.Check(valueProvider: valueProvider, error: out var error);

        Assert.That(result, Is.False);

        var typedError = error as EntityValidationError;
        Assert.That(typedError?.ValidationErrors, Has.Count.EqualTo(1));
    }

    [Test]
    public void ReturnTrue_WhenCheckedValueIsValid()
    {
        var entity = new TestValidatableEntity { ShouldSucceed = true };
        var asserter = new ValidatableEntityAsserter<TestValidatableEntity>();
        var valueProvider = TestValue.Of(entity);

        var result = asserter.Check(valueProvider: valueProvider, error: out var error);

        Assert.That(result, Is.True);
        Assert.That(error, Is.Null);
    }
}

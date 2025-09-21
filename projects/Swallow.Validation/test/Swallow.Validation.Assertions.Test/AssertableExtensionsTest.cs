namespace Swallow.Validation;

using Errors;
using NUnit.Framework;
using Utils;

[TestFixture]
public sealed class AssertableExtensionsTest
{
    [Test]
    public void ProduceGivenErrorInResult_WhenErrorFunctionIsGiven()
    {
        var error = RangeValidationError<int>.FromLowerBound(value: 5, isInclusive: false);
        var result = AssertionTester.Assert(value: 5, assertion: v => v.Satisfies(predicate: x => x > 5, errorFunc: _ => error));

        Assert.That(result.Errors, Is.EquivalentTo(new[] { error }));
    }

    [Test]
    public void ProductGenericValidationError_WhenMessageIsGiven()
    {
        var result = AssertionTester.Assert(value: 5, assertion: v => v.Satisfies(predicate: x => x > 5, message: "be larger than 5"));

        Assert.That(result.Errors, Has.Count.EqualTo(1));
        Assert.That(result.Errors[0].Message, Does.Contain("be larger than 5"));
    }
}

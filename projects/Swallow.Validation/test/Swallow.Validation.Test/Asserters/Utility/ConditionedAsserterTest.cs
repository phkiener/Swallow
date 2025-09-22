using NUnit.Framework;
using Swallow.Validation.Next.Asserters.Text;
using Swallow.Validation.Next.Helpers;

namespace Swallow.Validation.Next.Asserters.Utility;

[TestFixture]
public sealed class ConditionedAsserterTest
{
    [Test]
    public void SkipsAsserter_WhenConditionIsNotMet()
    {
        var assertersCalled = 0;
        var validator = Validator.Check()
            .That("", TestAsserter<string>.Succeed(() => assertersCalled += 1).When(false))
            .That("", TestAsserter<string>.Succeed(() => assertersCalled += 1).When(static () => false))
            .That("", TestAsserter<string>.Succeed(() => assertersCalled += 1).Unless(true))
            .That("", TestAsserter<string>.Succeed(() => assertersCalled += 1).Unless(static () => true));

        validator.OrThrow();
        Assert.That(assertersCalled, Is.EqualTo(0));
    }

    [Test]
    public void RunsAsserter_WhenConditionIsMet()
    {
        var assertersCalled = 0;
        var validator = Validator.Check()
            .That("", TestAsserter<string>.Succeed(() => assertersCalled += 1).When(true))
            .That("", TestAsserter<string>.Succeed(() => assertersCalled += 1).When(static () => true))
            .That("", TestAsserter<string>.Succeed(() => assertersCalled += 1).Unless(false))
            .That("", TestAsserter<string>.Succeed(() => assertersCalled += 1).Unless(static () => false));

        validator.OrThrow();
        Assert.That(assertersCalled, Is.EqualTo(4));
    }

    [Test]
    public void ReturnsErrorOfInnerAsserter()
    {
        var innerAsserter = new IsNotEmptyAsserter();
        var wrappedAsserter = innerAsserter.When(false);

        Assert.That(wrappedAsserter.Error, Is.SameAs(innerAsserter.Error));
    }
}

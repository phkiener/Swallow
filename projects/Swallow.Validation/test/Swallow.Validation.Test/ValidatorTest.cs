using NUnit.Framework;
using Swallow.Validation.Next.Asserters;
using Swallow.Validation.Next.Asserters.Text;
using Swallow.Validation.Next.Helpers;

namespace Swallow.Validation.Next;

[TestFixture]
public sealed class ValidatorTest
{
    [Test]
    public void DoesNothing_WhenNoRulesHaveBeenConfigured()
    {
        Assert.DoesNotThrow(() => Validator.Check().OrThrow());
    }

    [Test]
    public void DoesNotThrow_WhenAssertionPasses()
    {
        var validator = Validator.Check().That("hello", Satisfies.NotEmpty);

        Assert.DoesNotThrow(() => validator.OrThrow());
    }

    [Test]
    public void Throws_WhenAssertionFails()
    {
        var validator = Validator.Check().That("", Satisfies.NotEmpty);

        var exception = Assert.Throws<ValidationFailedException>(() => validator.OrThrow());
        Assert.That(exception?.ValidationErrors, Has.One.InstanceOf<EmptyString>());
    }

    [Test]
    public void EvaluatesAllAsserters_WhenErrorHandlingIsContinue()
    {
        const string first = "hello";
        const string second = "goodbye";

        var assertersCalled = 0;
        var validator = Validator.Check(ErrorHandling.Continue)
            .That(first, TestAsserter<string>.Fail(() => assertersCalled += 1))
            .That(first, TestAsserter<string>.Succeed(() => assertersCalled += 1))
            .That(second, TestAsserter<string>.Fail(() => assertersCalled += 1))
            .That(second, TestAsserter<string>.Succeed(() => assertersCalled += 1));

        Assert.Throws<ValidationFailedException>(validator.OrThrow);
        Assert.That(assertersCalled, Is.EqualTo(4));
    }

    [Test]
    public void SkipsRulesForSameName_ByDefault()
    {
        const string first = "hello";
        const string second = "goodbye";

        var assertersCalled = 0;
        var validator = Validator.Check()
            .That(first, TestAsserter<string>.Fail(() => assertersCalled += 1))
            .That(first, TestAsserter<string>.Succeed(() => assertersCalled += 1))
            .That(second, TestAsserter<string>.Fail(() => assertersCalled += 1))
            .That(second, TestAsserter<string>.Succeed(() => assertersCalled += 1));

        Assert.Throws<ValidationFailedException>(validator.OrThrow);
        Assert.That(assertersCalled, Is.EqualTo(2));
    }

    [Test]
    public void SkipsAllRules_WhenErrorHandlingIsSkipAll()
    {
        const string first = "hello";
        const string second = "goodbye";

        var assertersCalled = 0;
        var validator = Validator.Check(ErrorHandling.SkipAll)
            .That(first, TestAsserter<string>.Fail(() => assertersCalled += 1))
            .That(first, TestAsserter<string>.Succeed(() => assertersCalled += 1))
            .That(second, TestAsserter<string>.Fail(() => assertersCalled += 1))
            .That(second, TestAsserter<string>.Succeed(() => assertersCalled += 1));

        Assert.Throws<ValidationFailedException>(validator.OrThrow);
        Assert.That(assertersCalled, Is.EqualTo(1));
    }
}

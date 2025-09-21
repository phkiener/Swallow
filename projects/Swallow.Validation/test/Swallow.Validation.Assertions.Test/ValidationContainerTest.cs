namespace Swallow.Validation;

using System;
using NUnit.Framework;
using TestUtils;
using Utils;

[TestFixture]
public sealed class ValidationContainerTest
{
    [Test]
    public void ReturnTrue_WhenNoAssertersAreRegistered()
    {
        var container = new ValidationContainer();
        var result = container.Check(valueProvider: TestValue.Of<object>(5), error: out var error);

        Assert.That(result, Is.True);
        Assert.That(error, Is.Null);
    }

    [Test]
    public void ReturnTrue_WhenRegisteredAsserterDoesNotMatchTypeOfGivenValue()
    {
        var container = new ValidationContainer();
        container.Register(TestAsserter.Failing<int>());

        var result = container.Check(valueProvider: TestValue.Of<object>("hello"), error: out var error);

        Assert.That(result, Is.True);
        Assert.That(error, Is.Null);
    }

    [Test]
    public void ReturnResultOfRegisteredAsserter_WhenGivenTypeMatchesTypeOfAsserter()
    {
        var asserter = TestAsserter.Failing<int>();
        var container = new ValidationContainer([asserter]);

        var result = container.Check(valueProvider: TestValue.Of<object>(5), error: out var error);


        Assert.That(result, Is.False);
        Assert.That(error, Is.EqualTo(asserter.GeneratedError));
    }

    [Test]
    public void RunAllRegisteredAssertersForTypeOfGivenValue()
    {
        var firstAsserter = TestAsserter.Succeeding<int>();
        var secondAsserter = TestAsserter.Succeeding<int>();
        var container = new ValidationContainer([firstAsserter, secondAsserter]);

        container.Check(valueProvider: TestValue.Of<object>(5), error: out _);

        Assert.That(firstAsserter.TimesCalled, Is.EqualTo(1));
        Assert.That(secondAsserter.TimesCalled, Is.EqualTo(1));
    }

    [Test]
    public void ThrowException_WhenGivenObjectIsNotAnAsserter()
    {
        var container = new ValidationContainer();
        Assert.Throws<ArgumentException>(() => container.Register("hello"));
    }

    [Test]
    public void RegisterAsserter_WhenGivenAsObject()
    {
        var asserter = TestAsserter.Succeeding<int>();

        var container = new ValidationContainer();
        container.Register((object)asserter);

        AssertionTester.Assert<object>(value: 5, assertion: v => v.Satisfies(container));
        Assert.That(asserter.TimesCalled, Is.EqualTo(1));
    }

    [Test]
    public void RegisterTheGivenAsserters()
    {
        var firstAsserter = TestAsserter.Succeeding<int>();

        var container = new ValidationContainer([firstAsserter]);

        AssertionTester.Assert<object>(value: 5, assertion: v => v.Satisfies(container));
        Assert.That(firstAsserter.TimesCalled, Is.EqualTo(1));
    }

    [Test]
    public void ThrowException_WhenArgumentIsNotAnAsserter()
    {
        Assert.Throws<ArgumentException>(() => new ValidationContainer(["hello"]));
    }
}

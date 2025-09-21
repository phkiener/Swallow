namespace Swallow.Validation;

using FluentAssertions;
using NUnit.Framework;
using TestUtils;

[TestFixture]
public sealed class ConditionableExtensionsTest
{
    [Test]
    [TestCase(arg1: true, arg2: false)]
    [TestCase(arg1: false, arg2: true)]
    public void RunAssertionOnFalse_WhenGivenAsValue(bool conditionValue, bool asserterShouldBeCalled)
    {
        var asserter = TestAsserter.Succeeding<int>();

        Validator.Check()
            .That(() => 5).Satisfies(asserter).Unless(conditionValue)
            .Result();

        var expectedNumberOfCalls = asserterShouldBeCalled ? 1 : 0;
        Assert.That(asserter.TimesCalled, Is.EqualTo(expectedNumberOfCalls));
    }

    [Test]
    [TestCase(arg1: true, arg2: false)]
    [TestCase(arg1: false, arg2: true)]
    public void RunAssertionOnFalse_WhenGivenAsFunction(bool conditionValue, bool asserterShouldBeCalled)
    {
        var asserter = TestAsserter.Succeeding<int>();

        Validator.Check()
            .That(() => 5).Satisfies(asserter).Unless(() => conditionValue)
            .Result();

        var expectedNumberOfCalls = asserterShouldBeCalled ? 1 : 0;
        Assert.That(asserter.TimesCalled, Is.EqualTo(expectedNumberOfCalls));
    }

    [Test]
    [TestCase(arg1: true, arg2: true)]
    [TestCase(arg1: false, arg2: false)]
    public void RunAssertionOnTrue_WhenGivenAsValue(bool conditionValue, bool asserterShouldBeCalled)
    {
        var asserter = TestAsserter.Succeeding<int>();

        Validator.Check()
            .That(() => 5).Satisfies(asserter).When(conditionValue)
            .Result();

        var expectedNumberOfCalls = asserterShouldBeCalled ? 1 : 0;
        Assert.That(asserter.TimesCalled, Is.EqualTo(expectedNumberOfCalls));
    }

    [Test]
    [TestCase(arg1: true, arg2: true)]
    [TestCase(arg1: false, arg2: false)]
    public void RunAssertionOnTrue_WhenGivenAsFunction(bool conditionValue, bool asserterShouldBeCalled)
    {
        var asserter = TestAsserter.Succeeding<int>();

        Validator.Check()
            .That(() => 5).Satisfies(asserter).When(() => conditionValue)
            .Result();

        var expectedNumberOfCalls = asserterShouldBeCalled ? 1 : 0;
        asserter.TimesCalled.Should().Be(expectedNumberOfCalls);
    }
}

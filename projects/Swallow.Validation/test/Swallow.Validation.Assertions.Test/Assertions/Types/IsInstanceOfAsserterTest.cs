#nullable enable
using NUnit.Framework;
using Swallow.Validation.Assertions.Text;
using Swallow.Validation.Utils;

namespace Swallow.Validation.Assertions.Types;

[TestFixture]
public sealed class IsInstanceOfAsserterTest
{
    [Test]
    public void ReportsSuccess_WhenValueIsOfCorrectType()
    {
        var asserter = new IsInstanceOfAsserter<string>();
        var result = AssertionTester.Assert("hello", asserter, out _);

        Assert.That(result, Is.True);
    }

    [Test]
    public void ReportsError_WhenValueIsOfDifferentType()
    {
        var asserter = new IsInstanceOfAsserter<string>();
        var result = AssertionTester.Assert(123, asserter, out _);

        Assert.That(result, Is.False);
    }

    [Test]
    public void ReportsSuccess_WhenValueIsOfDerivedType_OnAllowDerivedTypes()
    {
        var asserter = new IsInstanceOfAsserter<BaseType>(allowDerivedTypes: true);
        var result = AssertionTester.Assert(new DerivedType(), asserter, out _);

        Assert.That(result, Is.True);
    }

    [Test]
    public void ReportsError_WhenValueIsOfDerivedType_OnDisallowDerivedTypes()
    {
        var asserter = new IsInstanceOfAsserter<BaseType>(allowDerivedTypes: false);
        var result = AssertionTester.Assert(new DerivedType(), asserter, out _);

        Assert.That(result, Is.False);
    }

    [Test]
    public void ReportedError_HasCorrectMessage()
    {
        var asserter = new IsInstanceOfAsserter<string>();
        AssertionTester.Assert(123, asserter, out var error);

        var typedError = error as WrongType;
        Assert.That(typedError?.Message, Is.EqualTo("value is not of type string"));
    }

    private record BaseType;

    private sealed record DerivedType : BaseType;
}

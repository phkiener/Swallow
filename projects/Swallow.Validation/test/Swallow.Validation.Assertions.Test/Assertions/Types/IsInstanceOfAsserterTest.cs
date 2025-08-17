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

    [Test]
    public void ReportedError_ContainsFriendlyNameForKeywords()
    {
        Assert.That(new WrongType(typeof(bool)).Message, Is.EqualTo(" is not of type bool"));
        Assert.That(new WrongType(typeof(byte)).Message, Is.EqualTo(" is not of type byte"));
        Assert.That(new WrongType(typeof(sbyte)).Message, Is.EqualTo(" is not of type sbyte"));
        Assert.That(new WrongType(typeof(short)).Message, Is.EqualTo(" is not of type short"));
        Assert.That(new WrongType(typeof(ushort)).Message, Is.EqualTo(" is not of type ushort"));
        Assert.That(new WrongType(typeof(int)).Message, Is.EqualTo(" is not of type int"));
        Assert.That(new WrongType(typeof(uint)).Message, Is.EqualTo(" is not of type uint"));
        Assert.That(new WrongType(typeof(long)).Message, Is.EqualTo(" is not of type long"));
        Assert.That(new WrongType(typeof(ulong)).Message, Is.EqualTo(" is not of type ulong"));
        Assert.That(new WrongType(typeof(float)).Message, Is.EqualTo(" is not of type float"));
        Assert.That(new WrongType(typeof(double)).Message, Is.EqualTo(" is not of type double"));
        Assert.That(new WrongType(typeof(decimal)).Message, Is.EqualTo(" is not of type decimal"));
        Assert.That(new WrongType(typeof(char)).Message, Is.EqualTo(" is not of type char"));
        Assert.That(new WrongType(typeof(string)).Message, Is.EqualTo(" is not of type string"));
        Assert.That(new WrongType(typeof(void)).Message, Is.EqualTo(" is not of type void"));
        Assert.That(new WrongType(typeof(object)).Message, Is.EqualTo(" is not of type object"));
    }

    [Test]
    public void ReportedError_ContainsFriendlyNameForGenericTypes()
    {
        Assert.That(new WrongType(typeof(List<string>)).Message, Is.EqualTo(" is not of type List<string>"));
        Assert.That(new WrongType(typeof(Dictionary<int, string>)).Message, Is.EqualTo(" is not of type Dictionary<int, string>"));
        Assert.That(new WrongType(typeof(IEnumerable<IComparable<DateTime>>)).Message, Is.EqualTo(" is not of type IEnumerable<IComparable<DateTime>>"));
    }

    private record BaseType;

    private sealed record DerivedType : BaseType;
}

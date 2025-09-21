#nullable enable
using NUnit.Framework;

namespace Swallow.Validation.V2.Types;

[TestFixture]
public sealed class IsInstanceOfAsserterTest
{
    [Test]
    public void ReportsSuccess_WhenValueIsOfCorrectType()
    {
        var asserter = IsInstanceOfAsserter.Equals<string>();
        Assert.That(asserter.IsValid("hello"), Is.True);
    }

    [Test]
    public void ReportsError_WhenValueIsOfDifferentType()
    {
        var asserter = IsInstanceOfAsserter.Equals<string>();
        Assert.That(asserter.IsValid(123), Is.False);
    }

    [Test]
    public void ReportsSuccess_WhenValueIsOfDerivedType_OnAllowDerivedTypes()
    {
        var asserter = IsInstanceOfAsserter.AssignableTo<BaseType>();
        Assert.That(asserter.IsValid(new DerivedType()), Is.True);
    }

    [Test]
    public void ReportsError_WhenValueIsOfDerivedType_OnDisallowDerivedTypes()
    {
        var asserter = IsInstanceOfAsserter.Equals<BaseType>();
        Assert.That(asserter.IsValid(new DerivedType()), Is.False);
    }

    [Test]
    public void ReturnsExpectedError()
    {
        var asserter = IsInstanceOfAsserter.Equals<string>();

        var typedError = asserter.Error as WrongType;
        Assert.That(typedError?.Message, Is.EqualTo("Value is not of type string"));
    }

    [Test]
    public void ReturnsExpectedError_ForKeywordTypes()
    {
        Assert.That(new WrongType(typeof(bool)).Message, Is.EqualTo("Value is not of type bool"));
        Assert.That(new WrongType(typeof(byte)).Message, Is.EqualTo("Value is not of type byte"));
        Assert.That(new WrongType(typeof(sbyte)).Message, Is.EqualTo("Value is not of type sbyte"));
        Assert.That(new WrongType(typeof(short)).Message, Is.EqualTo("Value is not of type short"));
        Assert.That(new WrongType(typeof(ushort)).Message, Is.EqualTo("Value is not of type ushort"));
        Assert.That(new WrongType(typeof(int)).Message, Is.EqualTo("Value is not of type int"));
        Assert.That(new WrongType(typeof(uint)).Message, Is.EqualTo("Value is not of type uint"));
        Assert.That(new WrongType(typeof(long)).Message, Is.EqualTo("Value is not of type long"));
        Assert.That(new WrongType(typeof(ulong)).Message, Is.EqualTo("Value is not of type ulong"));
        Assert.That(new WrongType(typeof(float)).Message, Is.EqualTo("Value is not of type float"));
        Assert.That(new WrongType(typeof(double)).Message, Is.EqualTo("Value is not of type double"));
        Assert.That(new WrongType(typeof(decimal)).Message, Is.EqualTo("Value is not of type decimal"));
        Assert.That(new WrongType(typeof(char)).Message, Is.EqualTo("Value is not of type char"));
        Assert.That(new WrongType(typeof(string)).Message, Is.EqualTo("Value is not of type string"));
        Assert.That(new WrongType(typeof(void)).Message, Is.EqualTo("Value is not of type void"));
        Assert.That(new WrongType(typeof(object)).Message, Is.EqualTo("Value is not of type object"));
    }

    [Test]
    public void ReturnsExpectedError_ForComplexTypes()
    {
        Assert.That(new WrongType(typeof(List<string>)).Message, Is.EqualTo("Value is not of type List<string>"));
        Assert.That(new WrongType(typeof(Dictionary<int, string>)).Message, Is.EqualTo("Value is not of type Dictionary<int, string>"));
        Assert.That(new WrongType(typeof(IEnumerable<IComparable<DateTime>>)).Message, Is.EqualTo("Value is not of type IEnumerable<IComparable<DateTime>>"));
    }

    private record BaseType;

    private sealed record DerivedType : BaseType;
}

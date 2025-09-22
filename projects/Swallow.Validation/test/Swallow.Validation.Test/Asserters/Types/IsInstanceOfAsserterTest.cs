using NUnit.Framework;

namespace Swallow.Validation.Next.Asserters.Types;

[TestFixture]
public sealed class IsInstanceOfAsserterTest
{
    [Test]
    public void ReportsSuccess_WhenValueIsOfCorrectType()
    {
        Assert.That(Satisfies.IsType<string>().IsValid("hello"), Is.True);
    }

    [Test]
    public void ReportsError_WhenValueIsOfDifferentType()
    {
        Assert.That(Satisfies.IsType<string>().IsValid(123), Is.False);
    }

    [Test]
    public void ReportsSuccess_WhenValueIsOfDerivedType_OnAllowDerivedTypes()
    {
        Assert.That(Satisfies.IsAssignableTo<BaseType>().IsValid(new DerivedType()), Is.True);
    }

    [Test]
    public void ReportsError_WhenValueIsOfDerivedType_OnDisallowDerivedTypes()
    {
        Assert.That(Satisfies.IsType<BaseType>().IsValid(new DerivedType()), Is.False);
    }

    [Test]
    public void ReturnsExpectedError()
    {
        var typedError = Satisfies.IsType<string>().Error as WrongType;
        Assert.That(typedError?.Message, Is.EqualTo("have type string"));
    }

    [Test]
    public void ReturnsExpectedError_ForKeywordTypes()
    {
        Assert.That(new WrongType(typeof(bool)).Message, Is.EqualTo("have type bool"));
        Assert.That(new WrongType(typeof(byte)).Message, Is.EqualTo("have type byte"));
        Assert.That(new WrongType(typeof(sbyte)).Message, Is.EqualTo("have type sbyte"));
        Assert.That(new WrongType(typeof(short)).Message, Is.EqualTo("have type short"));
        Assert.That(new WrongType(typeof(ushort)).Message, Is.EqualTo("have type ushort"));
        Assert.That(new WrongType(typeof(int)).Message, Is.EqualTo("have type int"));
        Assert.That(new WrongType(typeof(uint)).Message, Is.EqualTo("have type uint"));
        Assert.That(new WrongType(typeof(long)).Message, Is.EqualTo("have type long"));
        Assert.That(new WrongType(typeof(ulong)).Message, Is.EqualTo("have type ulong"));
        Assert.That(new WrongType(typeof(float)).Message, Is.EqualTo("have type float"));
        Assert.That(new WrongType(typeof(double)).Message, Is.EqualTo("have type double"));
        Assert.That(new WrongType(typeof(decimal)).Message, Is.EqualTo("have type decimal"));
        Assert.That(new WrongType(typeof(char)).Message, Is.EqualTo("have type char"));
        Assert.That(new WrongType(typeof(string)).Message, Is.EqualTo("have type string"));
        Assert.That(new WrongType(typeof(void)).Message, Is.EqualTo("have type void"));
        Assert.That(new WrongType(typeof(object)).Message, Is.EqualTo("have type object"));
    }

    [Test]
    public void ReturnsExpectedError_ForComplexTypes()
    {
        Assert.That(new WrongType(typeof(List<string>)).Message, Is.EqualTo("have type List<string>"));
        Assert.That(new WrongType(typeof(Dictionary<int, string>)).Message, Is.EqualTo("have type Dictionary<int, string>"));
        Assert.That(new WrongType(typeof(IEnumerable<IComparable<DateTime>>)).Message, Is.EqualTo("have type IEnumerable<IComparable<DateTime>>"));
    }

    private record BaseType;

    private sealed record DerivedType : BaseType;
}

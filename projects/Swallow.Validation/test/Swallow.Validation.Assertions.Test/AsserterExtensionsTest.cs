namespace Swallow.Validation;

using NUnit.Framework;
using TestUtils;

[TestFixture]
public sealed class AsserterExtensionsTest
{
    [Test]
    public void SetPropertyNameToValue()
    {
        var asserter = TestAsserter.Failing<int>();

        _ = asserter.Check(value: 5, error: out var error);

        Assert.That(error.PropertyName, Is.EqualTo("value"));
    }

    [Test]
    public void SetPropertyNameToGivenName()
    {
        var asserter = TestAsserter.Failing<int>();

        _ = asserter.Check(value: 5, name: "MyCoolInt", error: out var error);

        Assert.That(error.PropertyName, Is.EqualTo("MyCoolInt"));
    }

    [Test]
    public void SetPropertyNameToNameOfValueProvider()
    {
        var asserter = TestAsserter.Failing<string>();

        var valueProvider = TestValue.Of(value: "Value", name: "Name");
        _ = asserter.Check(valueProvider: valueProvider, error: out var error);

        Assert.That(error.PropertyName, Is.EqualTo(valueProvider.Name));
    }
}

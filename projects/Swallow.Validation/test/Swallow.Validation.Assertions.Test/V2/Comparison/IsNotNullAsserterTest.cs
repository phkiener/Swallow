#nullable enable
using NUnit.Framework;

namespace Swallow.Validation.V2.Comparison;

[TestFixture]
public sealed class IsNotNullAsserterTest
{
    private static readonly IsNotNullAsserter<int?> Asserter = new();

    [Test]
    public void ReportsSuccess_WhenValueIsNotNull()
    {
        Assert.That(Asserter.IsValid(1), Is.True);
    }

    [Test]
    public void ReportsError_WhenValueIsNull()
    {
        Assert.That(Asserter.IsValid(null), Is.False);
    }

    [Test]
    public void ReturnsExpectedError()
    {
        var typedError = Asserter.Error as ValueIsNull;
        Assert.That(typedError?.Message, Is.EqualTo("be not null"));
    }
}

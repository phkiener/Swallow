using NUnit.Framework;

namespace Swallow.Validation.V2.Utility;

[TestFixture]
public sealed class InvertedAsserterTest
{
    [Test]
    public void ReportsSuccess_WhenInnerAsserterReportsFailure()
    {
        var asserter = new InvertedAsserter<int>(TestAsserter<int>.Fail());
        Assert.That(asserter.IsValid(1), Is.True);
    }

    [Test]
    public void ReportsFailure_WhenInnerAsserterReportsSuccess()
    {
        var asserter = new InvertedAsserter<int>(TestAsserter<int>.Succeed());
        Assert.That(asserter.IsValid(1), Is.False);
    }

    [Test]
    public void ReturnsInvertedError()
    {
        var underlyingAsserter = TestAsserter<int>.Succeed();
        var asserter = new InvertedAsserter<int>(underlyingAsserter);

        var typedError = asserter.Error as InvertedError;
        Assert.That(typedError?.Error, Is.SameAs(underlyingAsserter.Error));
        Assert.That(typedError?.Message, Is.EqualTo("not be not empty"));
    }
}

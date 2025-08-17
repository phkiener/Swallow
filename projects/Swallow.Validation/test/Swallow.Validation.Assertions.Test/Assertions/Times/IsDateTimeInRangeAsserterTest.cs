#nullable enable
using NUnit.Framework;
using Swallow.Validation.Assertions.Times;
using Swallow.Validation.Utils;

namespace Swallow.Validation.Assertions.Text;

[TestFixture]
public sealed class IsDateTimeInRangeAsserterTest
{
    [Test]
    public void MatchesLowerBound()
    {
        var assertExclusive = new IsDateTimeInRangeAsserter(
            lowerBound: new DateTime(2000, 1, 1, 0, 0, 0),
            lowerBoundType: BoundsType.Exclusive);

        var assertInclusive = new IsDateTimeInRangeAsserter(
            lowerBound: new DateTime(2000, 1, 1, 0, 0, 0),
            lowerBoundType: BoundsType.Inclusive);

        var assertNothing = new IsDateTimeInRangeAsserter(
            lowerBound: null,
            lowerBoundType: BoundsType.Inclusive);

        var aboveLowerBound = new DateTime(2020, 1, 1, 0, 0, 0);
        var onLowerBound = new DateTime(2000, 1, 1, 0, 0, 0);
        var belowLowerBound = new DateTime(1970, 1, 1, 0, 0, 0);

        Assert.That(AssertionTester.Assert(onLowerBound, assertExclusive, out _), Is.False);
        Assert.That(AssertionTester.Assert(aboveLowerBound, assertExclusive, out _), Is.True);
        Assert.That(AssertionTester.Assert(belowLowerBound, assertExclusive, out _), Is.False);

        Assert.That(AssertionTester.Assert(onLowerBound, assertInclusive, out _), Is.True);
        Assert.That(AssertionTester.Assert(aboveLowerBound, assertInclusive, out _), Is.True);
        Assert.That(AssertionTester.Assert(belowLowerBound, assertInclusive, out _), Is.False);

        Assert.That(AssertionTester.Assert(onLowerBound, assertNothing, out _), Is.True);
        Assert.That(AssertionTester.Assert(aboveLowerBound, assertNothing, out _), Is.True);
        Assert.That(AssertionTester.Assert(belowLowerBound, assertNothing, out _), Is.True);
    }
    [Test]
    public void MatchesUpperBound()
    {
        var assertExclusive = new IsDateTimeInRangeAsserter(
            upperBound: new DateTime(2000, 1, 1, 0, 0, 0),
            upperBoundType: BoundsType.Exclusive);

        var assertInclusive = new IsDateTimeInRangeAsserter(
            upperBound: new DateTime(2000, 1, 1, 0, 0, 0),
            upperBoundType: BoundsType.Inclusive);

        var assertNothing = new IsDateTimeInRangeAsserter(
            upperBound: null,
            upperBoundType: BoundsType.Inclusive);

        var aboveLowerBound = new DateTime(2020, 1, 1, 0, 0, 0);
        var onLowerBound = new DateTime(2000, 1, 1, 0, 0, 0);
        var belowLowerBound = new DateTime(1970, 1, 1, 0, 0, 0);

        Assert.That(AssertionTester.Assert(onLowerBound, assertExclusive, out _), Is.False);
        Assert.That(AssertionTester.Assert(aboveLowerBound, assertExclusive, out _), Is.False);
        Assert.That(AssertionTester.Assert(belowLowerBound, assertExclusive, out _), Is.True);

        Assert.That(AssertionTester.Assert(onLowerBound, assertInclusive, out _), Is.True);
        Assert.That(AssertionTester.Assert(aboveLowerBound, assertInclusive, out _), Is.False);
        Assert.That(AssertionTester.Assert(belowLowerBound, assertInclusive, out _), Is.True);

        Assert.That(AssertionTester.Assert(onLowerBound, assertNothing, out _), Is.True);
        Assert.That(AssertionTester.Assert(aboveLowerBound, assertNothing, out _), Is.True);
        Assert.That(AssertionTester.Assert(belowLowerBound, assertNothing, out _), Is.True);
    }

    [Test]
    public void ReportedError_HasCorrectMessage()
    {
        var asserter = new IsDateTimeInRangeAsserter(
            lowerBound: new DateTime(2000, 1, 1, 0, 0, 0),
            upperBound: new DateTime(2010, 1, 1, 0, 0, 0),
            lowerBoundType: BoundsType.Inclusive,
            upperBoundType: BoundsType.Exclusive);

        AssertionTester.Assert(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local), asserter, out var error);

        var typedError = error as DateTimeNotInRange;
        Assert.That(typedError?.LowerBound, Is.EqualTo(new DateTime(2000, 1, 1, 0, 0, 0)));
        Assert.That(typedError?.UpperBound, Is.EqualTo(new DateTime(2010, 1, 1, 0, 0, 0)));
        Assert.That(typedError?.LowerBoundType, Is.EqualTo(BoundsType.Inclusive));
        Assert.That(typedError?.UpperBoundType, Is.EqualTo(BoundsType.Exclusive));
        Assert.That(typedError?.Message, Is.EqualTo("value is outside of the valid range"));
    }
}

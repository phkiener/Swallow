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
        var assertExclusive = IsDateTimeInRangeAsserter.After(new DateTime(2000, 1, 1, 0, 0, 0), boundsType: BoundsType.Exclusive);
        var assertInclusive = IsDateTimeInRangeAsserter.After(new DateTime(2000, 1, 1, 0, 0, 0), boundsType: BoundsType.Inclusive);
        var assertNothing = new IsDateTimeInRangeAsserter(lowerBound: null, lowerBoundType: BoundsType.Inclusive);

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
        var assertExclusive = IsDateTimeInRangeAsserter.Before(new DateTime(2000, 1, 1, 0, 0, 0), boundsType: BoundsType.Exclusive);
        var assertInclusive = IsDateTimeInRangeAsserter.Before(new DateTime(2000, 1, 1, 0, 0, 0), boundsType: BoundsType.Inclusive);
        var assertNothing = new IsDateTimeInRangeAsserter(lowerBound: null, lowerBoundType: BoundsType.Inclusive);

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
        var asserter = IsDateTimeInRangeAsserter.Between(
            start: new DateTime(2000, 1, 1, 0, 0, 0),
            end: new DateTime(2010, 1, 1, 0, 0, 0),
            boundsType: BoundsType.Inclusive);

        AssertionTester.Assert(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local), asserter, out var error);

        var typedError = error as DateTimeNotInRange;
        Assert.That(typedError?.LowerBound, Is.EqualTo(new DateTime(2000, 1, 1, 0, 0, 0)));
        Assert.That(typedError?.UpperBound, Is.EqualTo(new DateTime(2010, 1, 1, 0, 0, 0)));
        Assert.That(typedError?.LowerBoundType, Is.EqualTo(BoundsType.Inclusive));
        Assert.That(typedError?.UpperBoundType, Is.EqualTo(BoundsType.Inclusive));
        Assert.That(typedError?.Message, Is.EqualTo("value is outside of the valid range"));
    }
}

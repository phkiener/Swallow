#nullable enable
using NUnit.Framework;
using Swallow.Validation.Utils;

namespace Swallow.Validation.Assertions.Comparison;

[TestFixture]
public sealed class IsInRangeAsserterTest
{
    [Test]
    public void MatchesLowerBound()
    {
        var assertExclusive = IsInRangeAsserter.After(new DateTime(2000, 1, 1, 0, 0, 0), boundsType: BoundsType.Exclusive);
        var assertInclusive = IsInRangeAsserter.After(new DateTime(2000, 1, 1, 0, 0, 0), boundsType: BoundsType.Inclusive);
        var aboveLowerBound = new DateTime(2020, 1, 1, 0, 0, 0);
        var onLowerBound = new DateTime(2000, 1, 1, 0, 0, 0);
        var belowLowerBound = new DateTime(1970, 1, 1, 0, 0, 0);

        Assert.That(AssertionTester.Assert(onLowerBound, assertExclusive, out _), Is.False);
        Assert.That(AssertionTester.Assert(aboveLowerBound, assertExclusive, out _), Is.True);
        Assert.That(AssertionTester.Assert(belowLowerBound, assertExclusive, out _), Is.False);

        Assert.That(AssertionTester.Assert(onLowerBound, assertInclusive, out _), Is.True);
        Assert.That(AssertionTester.Assert(aboveLowerBound, assertInclusive, out _), Is.True);
        Assert.That(AssertionTester.Assert(belowLowerBound, assertInclusive, out _), Is.False);
    }
    [Test]
    public void MatchesUpperBound()
    {
        var assertExclusive = IsInRangeAsserter.Before(new DateTime(2000, 1, 1, 0, 0, 0), boundsType: BoundsType.Exclusive);
        var assertInclusive = IsInRangeAsserter.Before(new DateTime(2000, 1, 1, 0, 0, 0), boundsType: BoundsType.Inclusive);

        var aboveLowerBound = new DateTime(2020, 1, 1, 0, 0, 0);
        var onLowerBound = new DateTime(2000, 1, 1, 0, 0, 0);
        var belowLowerBound = new DateTime(1970, 1, 1, 0, 0, 0);

        Assert.That(AssertionTester.Assert(onLowerBound, assertExclusive, out _), Is.False);
        Assert.That(AssertionTester.Assert(aboveLowerBound, assertExclusive, out _), Is.False);
        Assert.That(AssertionTester.Assert(belowLowerBound, assertExclusive, out _), Is.True);

        Assert.That(AssertionTester.Assert(onLowerBound, assertInclusive, out _), Is.True);
        Assert.That(AssertionTester.Assert(aboveLowerBound, assertInclusive, out _), Is.False);
        Assert.That(AssertionTester.Assert(belowLowerBound, assertInclusive, out _), Is.True);
    }

    [Test]
    public void ReportedError_HasCorrectMessage()
    {
        var asserter = IsInRangeAsserter.Between(
            start: new DateTime(2000, 1, 1, 0, 0, 0),
            end: new DateTime(2010, 1, 1, 0, 0, 0),
            boundsType: BoundsType.Inclusive);

        AssertionTester.Assert(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local), asserter, out var error);

        var typedError = error as NotInRange<DateTime>;
        Assert.That(typedError?.LowerBound, Is.EqualTo(new DateTime(2000, 1, 1, 0, 0, 0)));
        Assert.That(typedError?.UpperBound, Is.EqualTo(new DateTime(2010, 1, 1, 0, 0, 0)));
        Assert.That(typedError?.LowerBoundType, Is.EqualTo(BoundsType.Inclusive));
        Assert.That(typedError?.UpperBoundType, Is.EqualTo(BoundsType.Inclusive));
        Assert.That(typedError?.Message, Is.EqualTo("value is outside of the valid range"));
    }
}

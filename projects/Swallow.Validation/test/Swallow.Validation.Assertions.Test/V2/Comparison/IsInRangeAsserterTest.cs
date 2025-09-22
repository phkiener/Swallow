#nullable enable
using NUnit.Framework;

namespace Swallow.Validation.V2.Comparison;

[TestFixture]
public sealed class IsInRangeAsserterTest
{
    private static readonly DateTime BelowLowerBound = new(year: 1970, month: 1, day: 1, hour: 0, minute: 0, second: 0);
    private static readonly DateTime OnLowerBound = new(year: 2000, month: 1, day: 1, hour: 0, minute: 0, second: 0);
    private static readonly DateTime AboveLowerBound = new(year: 2020, month: 1, day: 1, hour: 0, minute: 0, second: 0);

    [Test]
    public void MatchesLowerBound_Exclusive()
    {
        var asserter = IsInRangeAsserter.After(OnLowerBound, boundsType: BoundsType.Exclusive);

        Assert.That(asserter.IsValid(BelowLowerBound), Is.False);
        Assert.That(asserter.IsValid(OnLowerBound), Is.False);
        Assert.That(asserter.IsValid(AboveLowerBound), Is.True);
    }

    [Test]
    public void MatchesLowerBound_Inclusive()
    {
        var asserter = IsInRangeAsserter.After(OnLowerBound, boundsType: BoundsType.Inclusive);

        Assert.That(asserter.IsValid(BelowLowerBound), Is.False);
        Assert.That(asserter.IsValid(OnLowerBound), Is.True);
        Assert.That(asserter.IsValid(AboveLowerBound), Is.True);
    }

    [Test]
    public void MatchesUpperBound_Exclusive()
    {
        var asserter = IsInRangeAsserter.Before(OnLowerBound, boundsType: BoundsType.Exclusive);

        Assert.That(asserter.IsValid(BelowLowerBound), Is.True);
        Assert.That(asserter.IsValid(OnLowerBound), Is.False);
        Assert.That(asserter.IsValid(AboveLowerBound), Is.False);
    }

    [Test]
    public void MatchesUpperBound_Inclusive()
    {
        var asserter = IsInRangeAsserter.Before(OnLowerBound, boundsType: BoundsType.Inclusive);

        Assert.That(asserter.IsValid(BelowLowerBound), Is.True);
        Assert.That(asserter.IsValid(OnLowerBound), Is.True);
        Assert.That(asserter.IsValid(AboveLowerBound), Is.False);
    }

    [Test]
    public void ReturnsExpectedError()
    {
        var asserter = IsInRangeAsserter.Between(
            start: OnLowerBound,
            startBoundsType: BoundsType.Inclusive,
            end: AboveLowerBound,
            endBoundsType: BoundsType.Exclusive);

        var typedError = asserter.Error as NotInRange<DateTime>;
        Assert.That(typedError?.LowerBound?.Value, Is.EqualTo(OnLowerBound));
        Assert.That(typedError?.LowerBound?.IsInclusive, Is.True);
        Assert.That(typedError?.UpperBound?.Value, Is.EqualTo(AboveLowerBound));
        Assert.That(typedError?.UpperBound?.IsInclusive, Is.False);
        Assert.That(typedError?.Message, Is.EqualTo("be in range"));
    }
}

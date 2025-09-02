namespace Swallow.Console.Arguments;

[TestFixture]
public sealed class CommandlineArgumentsTest
{
    [Test]
    public void CanParse_UnixStyleFlag_ShortName()
    {
        var arguments = Arguments.Parse("foo -b");

        Assert.That(arguments.HasFlag(shortName: 'b'), Is.True);
        Assert.That(arguments.HasOption(shortName: 'b', out _), Is.False);
    }

    [Test]
    public void CanParse_UnixStyleFlag_LongName()
    {
        var arguments = Arguments.Parse("foo --bar");

        Assert.That(arguments.HasFlag(shortName: null, longName: "bar"), Is.True);
        Assert.That(arguments.HasOption(shortName: null, longName: "bar", value: out _), Is.False);
    }

    [Test]
    public void CanParse_UnixStyleOption_ShortName()
    {
        var arguments = Arguments.Parse("foo -b baz");

        Assert.That(arguments.HasFlag(shortName: 'b'), Is.False);
        Assert.That(arguments.HasOption(shortName: 'b', out var value), Is.True);
        Assert.That(value, Is.EqualTo("baz"));
    }

    [Test]
    public void CanParse_UnixStyleOption_LongName()
    {
        var arguments = Arguments.Parse("foo --bar baz");

        Assert.That(arguments.HasFlag(shortName: null, longName: "bar"), Is.False);
        Assert.That(arguments.HasOption(shortName: null, longName: "bar", value: out var value), Is.True);
        Assert.That(value, Is.EqualTo("baz"));
    }
}

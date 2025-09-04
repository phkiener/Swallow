namespace Swallow.Console.Arguments;

[TestFixture]
public sealed class CommandlineArgumentsTest
{
    [Test]
    public void CanParse_UnixStyleFlag_ShortName()
    {
        var arguments = Arguments.Parse("-b");

        AssertTokens(arguments, new ShortOption('b'));
    }

    [Test]
    public void CanParse_UnixStyleFlag_LongName()
    {
        var arguments = Arguments.Parse("--bar");

        AssertTokens(arguments, new LongOption("bar"));
    }

    [Test]
    public void CanParse_UnixStyleOption_ShortName()
    {
        var arguments = Arguments.Parse("-b baz");

        AssertTokens(arguments, new ShortOption('b'), new ParameterOrOptionValue("baz"));
    }

    [Test]
    public void CanParse_UnixStyleOption_LongName()
    {
        var arguments = Arguments.Parse("--bar baz");

        AssertTokens(arguments, new LongOption("bar"), new ParameterOrOptionValue("baz"));
    }

    [Test]
    public void CanParse_PlainParameter()
    {
        var arguments = Arguments.Parse("baz");

        AssertTokens(arguments, new Parameter("baz"));
    }

    private static void AssertTokens(CommandlineArguments arguments, params Token[] tokens)
    {
        Assert.That(arguments, Is.EqualTo(tokens));
    }
}

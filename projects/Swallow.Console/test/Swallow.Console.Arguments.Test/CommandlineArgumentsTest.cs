namespace Swallow.Console.Arguments;

[TestFixture]
public sealed class CommandlineArgumentsTest
{
    [Test]
    public void CanParse_UnixStyleFlag_ShortName()
    {
        var arguments = Arguments.Parse("-b");

        AssertTokens(arguments, Token.Option('b'));
    }

    [Test]
    public void CanParse_UnixStyleFlag_LongName()
    {
        var arguments = Arguments.Parse("--bar");

        AssertTokens(arguments, Token.Option("bar"));
    }

    [Test]
    public void CanParse_UnixStyleOption_ShortName()
    {
        var arguments = Arguments.Parse("-b baz");

        AssertTokens(arguments, Token.Option('b'), Token.ParameterOrOptionValue("baz"));
    }

    [Test]
    public void CanParse_UnixStyleOption_LongName()
    {
        var arguments = Arguments.Parse("--bar baz");

        AssertTokens(arguments, Token.Option("bar"), Token.ParameterOrOptionValue("baz"));
    }

    [Test]
    public void CanParse_PlainParameter()
    {
        var arguments = Arguments.Parse("baz");

        AssertTokens(arguments, Token.Parameter("baz"));
    }

    private static void AssertTokens(CommandlineArguments arguments, params Token[] tokens)
    {
        Assert.That(arguments, Is.EqualTo(tokens));
    }
}

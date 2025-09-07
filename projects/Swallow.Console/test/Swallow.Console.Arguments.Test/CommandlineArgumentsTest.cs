namespace Swallow.Console.Arguments;

[TestFixture]
public sealed class CommandlineArgumentsTest
{
    [Test]
    public void UnixStyleFlags()
    {
        AssertTokens("-b", Token.Option("b"));
        AssertTokens("-abc", Token.Option("a"), Token.Option("b"), Token.Option("c"));
        AssertTokens("--bar", Token.Option("bar"));
    }

    [Test]
    public void UnixStyleOptions()
    {
        AssertTokens("-b foo", Token.Option("b"), Token.ParameterOrOptionValue("foo"));
        AssertTokens("-abc foo", Token.Option("a"), Token.Option("b"), Token.Option("c"), Token.ParameterOrOptionValue("foo"));
        AssertTokens("--bar foo", Token.Option("bar"), Token.ParameterOrOptionValue("foo"));
        AssertTokens("--bar=foo", Token.Option("bar"), Token.OptionValue("foo"));
    }

    [Test]
    public void WindowsStyleFlags()
    {
        AssertTokens("/b", Token.Option("b"));
        AssertTokens("/bar", Token.Option("bar"));
    }

    [Test]
    public void WindowsStyleOptions()
    {
        AssertTokens("/b:foo", Token.Option("b"), Token.OptionValue("foo"));
        AssertTokens("/bar:foo", Token.Option("bar"), Token.OptionValue("foo"));
    }

    [Test]
    public void DoubleDashLimiter()
    {
        AssertTokens("--foo -- bar", Token.Option("foo"), Token.Parameter("bar"));
        AssertTokens("--foo -- --bar", Token.Option("foo"), Token.Parameter("--bar"));
    }

    private static void AssertTokens(string input, params Token[] tokens)
    {
        var parsed = Arguments.Parse(input);
        Assert.That(parsed, Is.EqualTo(tokens));
    }
}

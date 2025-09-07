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

    [Test]
    public void QuotedParameters()
    {
        AssertTokens(@"foo ""bar baz"" quuz", Token.Parameter("foo"), Token.Parameter("bar baz"), Token.Parameter("quuz"));
        AssertTokens("foo 'bar baz' quuz", Token.Parameter("foo"), Token.Parameter("bar baz"), Token.Parameter("quuz"));
        AssertTokens("mixed 'quotes \" here'", Token.Parameter("mixed"), Token.Parameter("quotes \" here"));
        AssertTokens(@"mixed ""quotes ' here""", Token.Parameter("mixed"), Token.Parameter("quotes ' here"));
    }

    [Test]
    public void EscapedCharacters()
    {
        AssertTokens(@"foo bar\ baz quuz", Token.Parameter("foo"), Token.Parameter("bar baz"), Token.Parameter("quuz"));
        AssertTokens(@"'quote \' character'", Token.Parameter("quote \' character"));
        AssertTokens(@"'quote \"" character'", Token.Parameter("quote \" character"));

        AssertTokens(@"'Just a \n'", Token.Parameter("Just a \n"));
        AssertTokens(@"'Just a \r'", Token.Parameter("Just a \r"));
        AssertTokens(@"'Just a \t'", Token.Parameter("Just a \t"));
    }

    private static void AssertTokens(string input, params Token[] tokens)
    {
        var parsed = Arguments.Parse(input);
        Assert.That(parsed, Is.EqualTo(tokens));
    }
}

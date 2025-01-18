namespace Swallow.Console.Arguments;

[TestFixture]
public sealed class ArgumentParserTest
{
    private sealed record EmptyOptions;

    [Test]
    public void EmptyOptionsCanBeParsed()
    {
        var options = ArgParse.Parse<EmptyOptions>([]);
        Assert.That(options, Is.EqualTo(new EmptyOptions()));
    }

    private sealed record SingleArgument(string Value);

    [Test]
    public void SingleArgumentCanBeParsed()
    {
        var options = ArgParse.Parse<SingleArgument>(["a"]);
        Assert.That(options, Is.EqualTo(new SingleArgument("a")));
    }

    private sealed record SingleIntArgument(int Value);

    [Test]
    public void SingleArgumentWithTypeConversionCanBeParsed()
    {
        var options = ArgParse.Parse<SingleIntArgument>(["123"]);
        Assert.That(options, Is.EqualTo(new SingleIntArgument(123)));
    }

}

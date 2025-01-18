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

    private sealed record MultipleArguments(int Value, string OtherValue);

    [Test]
    public void MultipleArgumentsCanBeParsed()
    {
        var options = ArgParse.Parse<MultipleArguments>(["404", "Not Found"]);
        Assert.That(options, Is.EqualTo(new MultipleArguments(404, "Not Found")));
    }

    private sealed record ArgumentAndOptions(string Argument)
    {
        public bool Enable { get; init; } = false;
    }

    [Test]
    public void ArgumentAndOptionsCanBeParsed()
    {
        var options = ArgParse.Parse<ArgumentAndOptions>(["--enable", "bla"]);
        Assert.That(options, Is.EqualTo(new ArgumentAndOptions("bla") { Enable = true }));
    }

}

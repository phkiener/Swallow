namespace Swallow.Console.Arguments;

[TestFixture]
public sealed class ArgumentParserTest
{
    private sealed record EmptyOptions;

    [Test]
    public void EmptyOptionsCanBeParsed()
    {
        AssertParse([], new EmptyOptions());
    }

    private sealed record SingleArgument(string Value);

    [Test]
    public void SingleArgumentCanBeParsed()
    {
        AssertParse(["a"], new SingleArgument("a"));
    }

    private sealed record SingleIntArgument(int Value);

    [Test]
    public void SingleArgumentWithTypeConversionCanBeParsed()
    {
        AssertParse(["123"], new SingleIntArgument(123));
    }

    private sealed record MultipleArguments(int Value, string OtherValue);

    [Test]
    public void MultipleArgumentsCanBeParsed()
    {
        AssertParse(["404", "Not Found"], new MultipleArguments(404, "Not Found"));
    }

    private sealed record ArgumentAndOptions(string Argument)
    {
        public bool Enable { get; init; } = false;
    }

    [Test]
    public void ArgumentAndOptionsCanBeParsed()
    {
        AssertParse(["--enable", "bla"], new ArgumentAndOptions("bla") { Enable = true });
    }

    private sealed record ArgumentAndvaluedOptions(string Argument)
    {
        public int? Age { get; init; }
    }

    [Test]
    public void ArgumentAndValuedOptionsCanBeParsed()
    {
        AssertParse(["--age", "12", "bla"], new ArgumentAndvaluedOptions("bla") { Age = 12 });
    }

    private static void AssertParse<TOptions>(string[] args, TOptions expected)
    {
        var parsed = ArgParse.Parse<TOptions>(args);
        Assert.That(parsed, Is.EqualTo(expected));
    }

}

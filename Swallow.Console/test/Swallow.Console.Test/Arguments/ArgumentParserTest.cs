namespace Swallow.Console.Arguments;

[TestFixture]
public sealed class ArgumentParserTest
{
    [Test]
    public void EmptyOptionsCanBeParsed() => new EmptyOptions().ShouldParseFrom([]);
    private sealed record EmptyOptions;

    [Test]
    public void SingleArgumentCanBeParsed() => new SingleArgument("a").ShouldParseFrom(["a"]);
    private sealed record SingleArgument(string Value);

    [Test]
    public void SingleArgumentWithTypeConversionCanBeParsed() => new SingleIntArgument(123).ShouldParseFrom(["123"]);
    private sealed record SingleIntArgument(int Value);

    [Test]
    public void MultipleArgumentsCanBeParsed() => new MultipleArguments(404, "Not Found").ShouldParseFrom(["404", "Not Found"]);
    private sealed record MultipleArguments(int Value, string OtherValue);

    [Test]
    public void ArgumentAndOptionsCanBeParsed() => new ArgumentAndOptions("bla") { Enable = true }.ShouldParseFrom(["--enable", "bla"]);
    private sealed record ArgumentAndOptions(string Argument)
    {
        public bool Enable { get; init; } = false;
    }

    [Test]
    public void ArgumentAndValuedOptionsCanBeParsed() => new ArgumentAndvaluedOptions("bla") { Age = 12 }.ShouldParseFrom(["--age", "12", "bla"]);
    private sealed record ArgumentAndvaluedOptions(string Argument)
    {
        public int? Age { get; init; }
    }
}

file static class Extensions
{
    public static void ShouldParseFrom<TOptions>(this TOptions expected, string[] args)
    {
        var parsed = ArgParse.Parse<TOptions>(args);
        Assert.That(parsed, Is.EqualTo(expected));
    }
}

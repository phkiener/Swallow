// ReSharper disable All
// Lotta things are unused here - we just need the definitions for reflection

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

    [Test]
    public void NestedCommandsCanBeParsed() => new NestedOption.Add("bla") { Common = true }.ShouldParseFrom<NestedOption>(["--common", "add", "bla"]);
    private abstract record NestedOption(string Subject)
    {
        public bool Common { get; init; } = false;

        public sealed record Add(string Subject) : NestedOption(Subject);
    }

    [Test]
    public void DeepNestedCommandsCanBeParsed() => new DeepNestedOption.Add.File("bla") { Common = true, State = "foo", Count = 1 }
        .ShouldParseFrom<DeepNestedOption>(["--common", "add", "--state", "foo", "file", "--count", "1", "bla"]);
    private abstract record DeepNestedOption(string Subject)
    {
        public bool Common { get; init; } = false;

        public abstract record Add(string Subject) : DeepNestedOption(Subject)
        {
            public string State { get; init; } = "";

            public sealed record File(string Path) : Add(Path)
            {
                public int Count { get; init; } = 0;
            }
        }
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

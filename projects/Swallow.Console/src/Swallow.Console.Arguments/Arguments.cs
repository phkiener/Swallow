using System.Text.RegularExpressions;

namespace Swallow.Console.Arguments;

/// <summary>
/// Parse command line arguments in a structured manner.
/// </summary>
public static partial class Arguments
{
    /// <summary>
    /// Parse the given <paramref name="args"/> into a queryable object.
    /// </summary>
    /// <param name="args">The CLI arguments to parse.</param>
    /// <returns>An object to query the arguments in a structured manner.</returns>
    public static IReadOnlyList<Token> Parse(params IReadOnlyList<string> args)
    {
        return Tokenize(args);
    }

    /// <summary>
    /// Parse the given <paramref name="input"/> into a queryable object.
    /// </summary>
    /// <param name="input">The input text to parse.</param>
    /// <returns>An object to query the arguments in a structured manner.</returns>
    /// <remarks>
    /// The input is split on whitespace and then passed into <see cref="Parse(string[])"/>.
    /// Whitespace will be ignored if it is inside quotes (<c>"</c> or <c>'</c>). Escape sequences
    /// are supported and will be transformed, i.e. <c>\n</c> will be turned into a newline character.
    /// A backslash-escaped space will not be used to separate the arguments and instead be copied
    /// over verbatim.
    /// </remarks>
    public static IReadOnlyList<Token> Parse(string input)
    {
        var splittedArguments = Split(input);

        return Parse(splittedArguments);
    }

    /// <summary>
    /// Parse the given <paramref name="args"/> into the type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to parse the arguments as.</typeparam>
    /// <param name="args">The CLI arguments to parse.</param>
    /// <returns>A parsed instance of <typeparamref name="T"/>.</returns>
    public static T Parse<T>(params string[] args)
    {
        throw new NotImplementedException("This is not implemented. Yet.");
    }

    private static List<string> Split(string input)
    {
        var arguments = new List<string>();

        var argumentStart = 0;
        char? quotingCharacter = null;
        var isEscaped = false;

        for (var i = 0; i < input.Length; i++)
        {
            if (isEscaped)
            {
                isEscaped = false;
                continue;
            }

            if (input[i] == '\\')
            {
                isEscaped = true;
                continue;
            }

            if (quotingCharacter is not null)
            {
                if (input[i] == quotingCharacter)
                {
                    var argument = input[argumentStart..i];
                    arguments.Add(Unescape(argument));

                    quotingCharacter = null;
                    argumentStart = i + 1;
                }

                continue;
            }

            if (input[i] is '"' or '\'')
            {
                quotingCharacter = input[i];
                argumentStart = i + 1;
            }

            if (char.IsWhiteSpace(input[i]))
            {
                if (i - argumentStart >= 1)
                {
                    var argument = input[argumentStart..i];
                    arguments.Add(Unescape(argument));
                }

                argumentStart = i + 1;
            }
        }

        if (argumentStart < input.Length)
        {
            var argument = input[argumentStart..];
            arguments.Add(Unescape(argument));
        }

        return arguments;
    }

    private static IReadOnlyList<Token> Tokenize(IReadOnlyList<string> args)
    {
        var arguments = new List<Token>(args.Count);

        var canBeOptionValue = false;
        var forceParameter = false;
        foreach (var argument in args)
        {
            if (argument is "--")
            {
                forceParameter = true;
                continue;
            }

            if (forceParameter)
            {
                arguments.Add(Token.Parameter(argument));
                continue;
            }

            var unixStyleMatch = UnixStyleFlag().Match(argument);
            if (unixStyleMatch.Success)
            {
                var shortOption = unixStyleMatch.Groups["shortoption"];
                if (shortOption.Success)
                {
                    var options = shortOption.Captures.Select(static c => Token.Option(c.Value));
                    arguments.AddRange(options);

                    canBeOptionValue = true;
                    continue;
                }

                var longOption = unixStyleMatch.Groups["longoption"];
                if (longOption.Success)
                {
                    arguments.Add(Token.Option(longOption.Value));

                    var optionValue = unixStyleMatch.Groups["optionvalue"];
                    if (optionValue.Success)
                    {
                        arguments.Add(Token.OptionValue(optionValue.Value));
                        canBeOptionValue = false;
                    }
                    else
                    {
                        canBeOptionValue = true;
                    }

                    continue;
                }
            }

            var windowsStyleMatch = WindowsStyleFlag().Match(argument);
            if (windowsStyleMatch.Success)
            {
                var option = windowsStyleMatch.Groups["option"];
                if (option.Success)
                {
                    arguments.Add(Token.Option(option.Value));

                    var optionValue = windowsStyleMatch.Groups["optionvalue"];
                    if (optionValue.Success)
                    {
                        arguments.Add(Token.OptionValue(optionValue.Value));
                        canBeOptionValue = false;
                    }
                    else
                    {
                        canBeOptionValue = true;
                    }
                }

                continue;
            }

            var value = canBeOptionValue
                ? Token.ParameterOrOptionValue(argument)
                : Token.Parameter(argument);

            arguments.Add(value);
            canBeOptionValue = false;
        }

        return arguments;
    }

    private static string Unescape(string input)
    {
        return input
            .Replace(@"\n", "\n")
            .Replace(@"\r", "\r")
            .Replace(@"\t", "\t")
            .Replace(@"\ ", " ")
            .Replace(@"\\", "\\")
            .Replace(@"\""", "\"")
            .Replace(@"\'", "\'");
    }

    [GeneratedRegex(@"^(-(?<shortoption>\w)+|--(?<longoption>\w+)(=(?<optionvalue>.+))?)$")]
    private static partial Regex UnixStyleFlag();

    [GeneratedRegex(@"^/(?<option>\w+)(:(?<optionvalue>.+))?$")]
    private static partial Regex WindowsStyleFlag();
}

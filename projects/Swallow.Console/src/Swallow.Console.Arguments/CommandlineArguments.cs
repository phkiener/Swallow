using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Swallow.Console.Arguments;

/// <summary>
/// An object that you can query to get structured information about parsed commandline arguments.
/// </summary>
/// <seealso cref="Arguments.Parse(string[])"/>
public sealed partial class CommandlineArguments : IReadOnlyList<Token>
{
    private readonly Token[] tokens;

    private CommandlineArguments(Token[] tokens)
    {
        this.tokens = tokens;
    }

    /// <inheritdoc />
    public IEnumerator<Token> GetEnumerator() => tokens.AsEnumerable().GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => tokens.GetEnumerator();

    /// <inheritdoc />
    public int Count => tokens.Length;

    /// <inheritdoc />
    public Token this[int index] => tokens[index];

    /// <summary>
    /// Returns <c>true</c> if the parsed arguments contain the specified flag.
    /// </summary>
    /// <param name="shortName">The single letter that the flag corresponds to.</param>
    /// <param name="longName">The long name that the flag corresponds to.</param>
    /// <returns><c>true</c> if the flag was parsed, <c>false</c> otherwise.</returns>
    /// <remarks>
    /// A <em>flag</em> is an option passed without any value; consider the following invocation:
    /// <code>
    /// dotnet build --no-restore --runtime linux-arm64
    /// </code>
    ///
    /// <c>--no-restore</c> is a <em>flag</em> in this case, thus the following would return <c>true</c>:
    /// <code>
    /// arguments.HasFlag(null, "no-restore");
    /// </code>
    ///
    /// <c>--runtime</c> on the other hand has a value following immediately after and would <em>not</em>
    /// be considered a flag, but the arguments themselves don't know about it accepting a value. As
    /// such, it can <em>also</em> be considered a flag if <c>linux-arm64</c> is used as argument
    /// (or subcommand) instead of an option value. Thus, the following would return <c>true</c> as well:
    /// <code>
    /// arguments.HasFlag(null, "runtime");
    /// </code>
    /// </remarks>
    public bool HasFlag(char? shortName, string? longName = null)
    {
        foreach (var token in tokens)
        {
            if (token.Type is not TokenType.Option)
            {
                continue;
            }

            if (shortName.HasValue && token.Value == new string(shortName.Value, 1)
                || longName is not null && token.Value == longName)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns <c>true</c> if the parsed arguments contains the specified option.
    /// </summary>
    /// <param name="shortName">The single letter that the flag corresponds to.</param>
    /// <param name="longName">The long name that the flag corresponds to.</param>
    /// <param name="value">The option's passed value, verbatim as it was encountered.</param>
    /// <returns><c>true</c> if the option was parsed, <c>false</c> otherwise.</returns>
    /// <remarks>
    /// An <em>option</em> is a flag passed with a value immediately after; consider the following invocation:
    /// <code>
    /// dotnet build --no-restore --runtime linux-arm64
    /// </code>
    ///
    /// <c>--no-restore</c> is a <em>not</em> an option in this case, thus the following would return <c>false</c>:
    /// <code>
    /// arguments.HasOption(null, out _, longName: "no-restore");
    /// </code>
    ///
    /// <c>--runtime</c> on the other hand has a value following immediately after and is considered an opton.
    /// Thus, the following would return <c>true</c> and set the out-parameter to <c>"linux-arm64"</c>:
    /// <code>
    /// arguments.HasFlag(null, out var value, "runtime");
    /// </code>
    /// </remarks>
    public bool HasOption(char? shortName, [MaybeNullWhen(false)] out string value, string? longName = null)
    {
        for (var i = 0; i < tokens.Length; i++)
        {
            var token = tokens[i];
            if (token.Type is not TokenType.Option)
            {
                continue;
            }

            if (shortName.HasValue && token.Value == new string(shortName.Value, 1)
                || longName is not null && token.Value == longName)
            {
                if (tokens.Length > i)
                {
                    var nextToken = tokens[i + 1];
                    if (nextToken.Type.HasFlag(TokenType.OptionValue))
                    {

                        value = nextToken.Value;
                        return true;
                    }
                }
            }
        }

        value = null;
        return false;
    }

    internal static CommandlineArguments From(string[] args)
    {
        var arguments = new List<Token>(args.Length);

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

        return new CommandlineArguments(arguments.ToArray());
    }

    [GeneratedRegex(@"^(-(?<shortoption>\w)+|--(?<longoption>\w+)(=(?<optionvalue>.+))?)$")]
    private static partial Regex UnixStyleFlag();

    [GeneratedRegex(@"^/(?<option>\w+)(:(?<optionvalue>.+))?$")]
    private static partial Regex WindowsStyleFlag();
}

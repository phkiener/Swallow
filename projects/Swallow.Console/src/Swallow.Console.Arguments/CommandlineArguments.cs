using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Swallow.Console.Arguments;

/// <summary>
/// A token parsed for <see cref="CommandlineArguments"/>.
/// </summary>
public readonly record struct Token(TokenType Type, string Value)
{

    /// <summary>
    /// An option declared using a single character.
    /// </summary>
    /// <param name="name">The character of the parsed option.</param>
    public static Token Option(char name) => new(TokenType.Option, new string(name, 1));

    /// <summary>
    /// An long option declared using multiple characters.
    /// </summary>
    /// <param name="name">The parsed option.</param>
    public static Token Option(string name) => new(TokenType.Option, name);

    /// <summary>
    /// An argument that is unambiguously used as value for a preceding <see cref="Token.Option(string)"/>.
    /// </summary>
    /// <param name="value">The contained value.</param>
    public static Token OptionValue(string value) => new(TokenType.OptionValue, value);

    /// <summary>
    /// An argument that is unambiguously used as a standalone parameter.
    /// </summary>
    /// <param name="value">The contained value.</param>
    public static Token Parameter(string value) => new(TokenType.Parameter, value);

    /// <summary>
    /// /// An argument that - depending on context - can be used as value for an option
    /// (<see cref="Option(string)"/>) <em>or</em> as standalone parameter (<see cref="Parameter"/>). No
    /// differentiation can be made based on syntax alone.
    /// </summary>
    /// <param name="value">The contained value.</param>
    public static Token ParameterOrOptionValue(string value) => new(TokenType.ParameterOrOptionValue, value);
}

[Flags]
public enum TokenType
{
    Option = 0b0000_0001,
    OptionValue = 0b0000_0010,
    Parameter = 0b0010_0000,

    ParameterOrOptionValue = OptionValue | Parameter
}

/// <summary>
/// An object that you can query to get structured information about parsed commandline arguments.
/// </summary>
/// <seealso cref="Arguments.Parse(string[])"/>
public sealed class CommandlineArguments : IReadOnlyList<Token>
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

        foreach (var argument in args)
        {
            switch (argument)
            {
                case ['-', var letter] when char.IsLetter(letter):
                    arguments.Add(Token.Option(letter));
                    continue;

                case ['-', '-', .. var name]:
                    arguments.Add(Token.Option(name));
                    continue;

                default:
                    var lastToken = arguments.LastOrDefault();
                    arguments.Add(
                        lastToken is { Type: TokenType.Option }
                            ? Token.ParameterOrOptionValue(argument)
                            : Token.Parameter(argument));
                    break;
            }
        }

        return new CommandlineArguments(arguments.ToArray());
    }
}

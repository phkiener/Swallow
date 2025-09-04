using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Swallow.Console.Arguments;

/// <summary>
/// The common interface for tokens parsed from commandline arguments.
/// </summary>
public interface Token;

/// <summary>
/// An option declared using a single character.
/// </summary>
/// <param name="Character">The character of the parsed option.</param>
public sealed record ShortOption(char Character) : Token;

/// <summary>
/// An long option declared using multiple characters.
/// </summary>
/// <param name="Text">The parsed option.</param>
public sealed record LongOption(string Text) : Token;

/// <summary>
/// An argument that is unambiguously used as value for a preceding <see cref="ShortOption"/> or
/// <see cref="LongOption"/>.
/// </summary>
/// <param name="Value">The contained value.</param>
public sealed record OptionValue(string Value) : Token;

/// <summary>
/// An argument that - depending on context - can be used as value for an option
/// (<see cref="OptionValue"/>) <em>or</em> as standalone parameter (<see cref="Parameter"/>). No
/// differentiation can be made based on syntax alone.
/// </summary>
/// <param name="Value">The contained value.</param>
public sealed record ParameterOrOptionValue(string Value) : Token;

/// <summary>
/// An argument that is unambiguously used as a standalone parameter.
/// </summary>
/// <param name="Value">The contained value.</param>
public sealed record Parameter(string Value) : Token;

/// <summary>
/// An object that you can query to get structured information about parsed commandline arguments.
/// </summary>
/// <seealso cref="Arguments.Parse(string[])"/>
public sealed class CommandlineArguments : IReadOnlyList<Token>
{
    private readonly List<Token> tokens;

    private CommandlineArguments(List<Token> tokens)
    {
        this.tokens = tokens;
    }

    /// <inheritdoc />
    public IEnumerator<Token> GetEnumerator() => tokens.GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public int Count => tokens.Count;

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
        if (shortName is not null && tokens.OfType<ShortOption>().Any(f => f.Character == shortName))
        {
            return true;
        }

        if (longName is not null  && tokens.OfType<LongOption>().Any(f => f.Text == longName))
        {
            return true;
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
        if (shortName is not null)
        {
            var foundOption = tokens.OfType<ShortOption>().FirstOrDefault(f => f.Character == shortName);
            if (foundOption is not null)
            {
                var index = tokens.IndexOf(foundOption);
                if (index is not -1)
                {
                    var nextToken = tokens.ElementAtOrDefault(index + 1);
                    if (nextToken is ParameterOrOptionValue parameterOrOptionValue)
                    {
                        value = parameterOrOptionValue.Value;
                        return true;
                    }

                    if (nextToken is OptionValue optionValue)
                    {
                        value = optionValue.Value;
                        return true;
                    }
                }
            }
        }

        if (longName is not null)
        {
            var foundOption = tokens.OfType<LongOption>().FirstOrDefault(f => f.Text == longName);
            if (foundOption is not null)
            {
                var index = tokens.IndexOf(foundOption);
                if (index is not -1)
                {
                    var nextToken = tokens.ElementAtOrDefault(index + 1);
                    if (nextToken is ParameterOrOptionValue parameterOrOptionValue)
                    {
                        value = parameterOrOptionValue.Value;
                        return true;
                    }

                    if (nextToken is OptionValue optionValue)
                    {
                        value = optionValue.Value;
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
        var arguments = new List<Token>();

        foreach (var argument in args)
        {
            if (argument is ['-', var letter] && char.IsLetter(letter))
            {
                arguments.Add(new ShortOption(letter));
                continue;
            }

            if (argument is ['-', '-', .. var name])
            {
                arguments.Add(new LongOption(name));
                continue;
            }

            var lastArgument = arguments.LastOrDefault();
            if (lastArgument is ShortOption or LongOption)
            {
                arguments.Add(new ParameterOrOptionValue(argument));
            }
            else
            {
                arguments.Add(new Parameter(argument));
            }
        }

        return new CommandlineArguments(arguments);
    }
}

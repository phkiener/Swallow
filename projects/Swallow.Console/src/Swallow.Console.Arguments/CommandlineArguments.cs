using System.Diagnostics.CodeAnalysis;

namespace Swallow.Console.Arguments;

/// <summary>
/// An object that you can query to get structured information about parsed commandline arguments.
/// </summary>
/// <seealso cref="Arguments.Parse"/>
public sealed class CommandlineArguments
{
    private readonly List<Argument> arguments;

    private CommandlineArguments(List<Argument> arguments)
    {
        this.arguments = arguments;
    }

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
        if (shortName is not null && arguments.OfType<ShortFlag>().Any(f => f.Character == shortName))
        {
            return true;
        }

        if (longName is not null  && arguments.OfType<LongFlag>().Any(f => f.Text == longName))
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
            var foundOption = arguments.OfType<ShortOption>().FirstOrDefault(f => f.Character == shortName);
            if (foundOption is not null)
            {
                value = foundOption.Value;
                return true;
            }
        }

        if (longName is not null)
        {
            var foundOption = arguments.OfType<LongOption>().FirstOrDefault(f => f.Text == longName);
            if (foundOption is not null)
            {
                value = foundOption.Value;
                return true;
            }
        }

        value = null;
        return false;
    }

    internal static CommandlineArguments From(string[] args)
    {
        var arguments = new List<Argument>();

        foreach (var argument in args)
        {
            if (argument is ['-', var letter] && char.IsLetter(letter))
            {
                arguments.Add(new ShortFlag(letter));
                continue;
            }

            if (argument is ['-', '-', .. var name])
            {
                arguments.Add(new LongFlag(name));
                continue;
            }

            var parameter = new Parameter(argument);

            var lastArgument = arguments.LastOrDefault();
            if (lastArgument is ShortFlag { Character: var character })
            {
                arguments.Add(new ShortOption(character, argument));
            }

            if (lastArgument is LongFlag { Text: var text })
            {
                arguments.Add(new LongOption(text, argument));
            }

            arguments.Add(parameter);
        }

        return new CommandlineArguments(arguments);
    }

    private interface Argument;

    private sealed record ShortFlag(char Character) : Argument;
    private sealed record LongFlag(string Text) : Argument;
    private sealed record ShortOption(char Character, string Value) : Argument;
    private sealed record LongOption(string Text, string Value) : Argument;
    private sealed record Parameter(string Value) : Argument;
}

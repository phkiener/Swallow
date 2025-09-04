namespace Swallow.Console.Arguments;

/// <summary>
/// Parse command line arguments in a structured manner.
/// </summary>
public static class Arguments
{
    /// <summary>
    /// Parse the given <paramref name="args"/> into a queryable object.
    /// </summary>
    /// <param name="args">The CLI arguments to parse.</param>
    /// <returns>An object to query the arguments in a structured manner.</returns>
    public static CommandlineArguments Parse(params string[] args)
    {
        return CommandlineArguments.From(args);
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
    public static CommandlineArguments Parse(string input)
    {
        var arguments = new List<string>();

        var argumentStart = 0;
        char? quotingCharacter = null;
        var isEscaped = false;

        for (var i = 0; i < input.Length; i++)
        {
            if (char.IsWhiteSpace(input[i]) && !isEscaped && quotingCharacter is null)
            {
                if (i - argumentStart >= 1)
                {
                    arguments.Add(input[argumentStart..i]);
                }

                argumentStart = i + 1;
                continue;
            }
        }

        if (argumentStart < input.Length)
        {
            arguments.Add(input[argumentStart..]);
        }

        return Parse(arguments.ToArray());
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
}

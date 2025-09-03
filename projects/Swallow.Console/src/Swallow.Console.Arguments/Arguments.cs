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

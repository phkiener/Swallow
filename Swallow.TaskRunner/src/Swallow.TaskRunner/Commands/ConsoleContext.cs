namespace Swallow.TaskRunner.Commands;

public sealed class ConsoleContext : ICommandContext
{
    public TextWriter Output => Console.Out;
    public TextWriter Error => Console.Error;
    public string CurrentDirectory => Environment.CurrentDirectory;
}

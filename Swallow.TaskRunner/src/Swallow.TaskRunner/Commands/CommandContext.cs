namespace Swallow.TaskRunner.Commands;

public sealed class CommandContext(TextWriter output, TextWriter error)
{
    public TextWriter Output { get; } = output;
    public TextWriter Error { get; } = error;
}

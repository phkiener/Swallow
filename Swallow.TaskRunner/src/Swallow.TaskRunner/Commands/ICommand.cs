namespace Swallow.TaskRunner.Commands;

public interface ICommand
{
    public Task<int> Run(CommandContext console, string[] args);
}

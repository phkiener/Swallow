namespace Swallow.TaskRunner.Commands;

public interface ICommand
{
    public Task<int> Run(ICommandContext console, string[] args);
}

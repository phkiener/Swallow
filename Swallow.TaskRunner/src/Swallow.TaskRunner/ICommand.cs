namespace Swallow.TaskRunner;

public interface ICommand
{
    public Task<int> RunAsync(ICommandContext console, string[] args);
}

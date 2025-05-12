namespace Swallow.TaskRunner.Abstractions;

public interface ICommand
{
    public Task<int> RunAsync(ICommandContext console, string[] args);
}

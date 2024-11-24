namespace Swallow.TaskRunner.Abstractions;

public interface ITask
{
    public Task<int> RunAsync(ICommandContext console);
}

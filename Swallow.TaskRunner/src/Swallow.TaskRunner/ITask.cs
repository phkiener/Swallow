namespace Swallow.TaskRunner;

public interface ITask
{
    public Task<int> RunAsync(ICommandContext console);
}

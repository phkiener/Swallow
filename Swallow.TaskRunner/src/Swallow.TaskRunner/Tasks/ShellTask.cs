namespace Swallow.TaskRunner.Tasks;

public class ShellTask(string commandLine) : ITask
{
    public async Task<int> RunAsync(ICommandContext console)
    {
        return await console.Execute(commandLine);
    }
}

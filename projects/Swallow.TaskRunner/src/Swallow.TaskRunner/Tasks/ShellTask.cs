using Swallow.TaskRunner.Abstractions;

namespace Swallow.TaskRunner.Tasks;

public class ShellTask(string commandLine) : ITask
{
    public string Commandline { get; } = commandLine;

    public async Task<int> RunAsync(ICommandContext console)
    {
        return await console.Execute(Commandline);
    }
}

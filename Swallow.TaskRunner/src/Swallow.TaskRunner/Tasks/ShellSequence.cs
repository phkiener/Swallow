namespace Swallow.TaskRunner.Tasks;

public class ShellSequence(params IEnumerable<string> commands) : ITask
{
    public IReadOnlyList<string> Commands { get; } = commands.ToList();

    public async Task<int> RunAsync(ICommandContext console)
    {
        foreach (var command in Commands)
        {
            var result = await console.Execute(command);
            if (result is not 0)
            {
                return result;
            }
        }

        return 0;
    }
}

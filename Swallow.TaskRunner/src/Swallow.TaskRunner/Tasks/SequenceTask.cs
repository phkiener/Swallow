namespace Swallow.TaskRunner.Tasks;

public class SequenceTask(params IEnumerable<ITask> tasks) : ITask
{
    public IReadOnlyList<ITask> Tasks { get; } = tasks.ToList();

    public async Task<int> RunAsync(ICommandContext console)
    {
        foreach (var tasks in Tasks)
        {
            var result = await tasks.RunAsync(console);
            if (result is not 0)
            {
                return result;
            }
        }

        return 0;
    }
}

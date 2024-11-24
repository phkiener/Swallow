using Swallow.TaskRunner.Abstractions;

namespace Swallow.TaskRunner;

public sealed class Manifest
{
    private readonly Dictionary<string, ITask> definedTasks = new(StringComparer.OrdinalIgnoreCase);

    public int Version => 1;
    public IReadOnlyDictionary<string, ITask> Tasks => definedTasks.AsReadOnly();

    public static Manifest Create()
    {
        return new Manifest();
    }

    public void AddTask(string name, ITask task)
    {
        definedTasks[name] = task;
    }

    public ITask? FindTask(string name)
    {
        return definedTasks.GetValueOrDefault(name);
    }
}

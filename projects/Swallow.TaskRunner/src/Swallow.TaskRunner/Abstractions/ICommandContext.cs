namespace Swallow.TaskRunner.Abstractions;

public interface ICommandContext
{
    public TextWriter Output { get; }
    public TextWriter Error { get; }
    public string CurrentDirectory { get; }
    public CancellationToken CancellationToken { get; }

    public Task<int> Execute(string command);
}

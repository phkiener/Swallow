namespace Swallow.TaskRunner.Commands;

public sealed class InvokeTask : ICommand
{
    public Task<int> Run(ICommandContext console, string[] args)
    {
        throw new NotImplementedException();
    }
}

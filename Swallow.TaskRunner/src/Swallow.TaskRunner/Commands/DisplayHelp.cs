﻿namespace Swallow.TaskRunner.Commands;

public sealed class DisplayHelp : ICommand
{
    public Task<int> RunAsync(ICommandContext console, string[] args)
    {
        throw new NotImplementedException();
    }
}

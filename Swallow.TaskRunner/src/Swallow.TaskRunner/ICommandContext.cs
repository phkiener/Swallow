﻿namespace Swallow.TaskRunner;

public interface ICommandContext
{
    public TextWriter Output { get; }
    public TextWriter Error { get; }
    public string CurrentDirectory { get; }
}
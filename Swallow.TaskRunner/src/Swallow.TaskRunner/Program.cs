﻿using Swallow.TaskRunner;
using Swallow.TaskRunner.Abstractions;
using Swallow.TaskRunner.Commands;

var (command, commandArguments) = Resolve(args);
using var context = new ConsoleContext();

return await command.RunAsync(context, commandArguments);

static (ICommand Command, string[] Args) Resolve(string[] args)
{
    return args switch
    {
        [] => (new DisplayHelp(), []),
        ["new-manifest"] => (new CreateManifest(), []),
        ["list"] => (new ListTasks(), []),
        ["add", var name, ..var rest] => (new AddTask(), [name, ..rest]),
        ["remove", var name, ..] => (new RemoveTask(), [name]),
        ["-v", ..] or ["--version", ..] => (new DisplayVersion(), []),
        ["-h", ..] or ["--help", ..] => (new DisplayHelp(), []),
        _ => (new InvokeTask(), args)
    };
}

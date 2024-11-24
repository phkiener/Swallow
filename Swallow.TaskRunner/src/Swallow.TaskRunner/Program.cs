using Swallow.TaskRunner.Commands;

var (command, commandArguments) = Resolve(args);
var context = new CommandContext(Console.Out, Console.Error);
return await command.Run(context, commandArguments);

static (ICommand Command, string[] Args) Resolve(string[] args)
{
    return args switch
    {
        [] => (new DisplayHelp(), []),
        ["new-manifest"] => (new CreateManifest(), []),
        ["-v", ..] or ["--version", ..] => (new DisplayVersion(), []),
        ["-h", ..] or ["--help", ..] => (new DisplayHelp(), []),
        _ => (new InvokeTask(), args)
    };
}

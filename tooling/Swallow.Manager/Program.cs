using Swallow.Manager.Commands;

var directory = Environment.CurrentDirectory;
while (!File.Exists(Path.Combine(directory, "Swallow.slnx")))
{
    directory = Directory.GetParent(directory)?.FullName;
    if (directory is null)
    {
        throw new InvalidOperationException("Unable to find repository root");
    }
}

switch (args)
{
    case ["generate", ..]:
        return Generate.Run(directory);

    case ["add", var projectName]:
        return Add.Run(directory, projectName);

    case ["publish", var publishName, var version]:
        return Publish.Run(directory, publishName, version);

    case ["-h", ..]:
    case ["--help"]:
        Help.Run();
        return 0;

    default:
        Help.Run();
        return 255;
}

// /assets         -> not in solution
// /lib            -> not in solution
// /shared         -> not in solution
// /PROJECT                    -> PROJECT/
//   /doc                      -> not in solution?
//   /example                  -> PROJECT/CSPROJ || CSPROJ
//   /src                      -> PROJECT/CSPROJ || CSPROJ
//   /test                     -> PROJECT/CSPROJ || CSPROJ
//   /Directory.Build.props    -> PROJECT/Additional items/ || Additional items/
//   /Directory.Build.targets  -> PROJECT/Additional items/ || Additional items/
//   /Directory.Packages.props -> PROJECT/Additional items/ || Additional items/
//   /LICENSE.txt              -> PROJECT/Additional items/ || Additional items/
//   /README.md                -> PROJECT/Additional items/ || Additional items/
//   /RELEASE_NOTES.md         -> PROJECT/Additional items/ || Additional items/
//   /PROJECT.sln              -> solution containing the specific projects
// /.editorconfig  -> Additional items/
// /.gitattributes -> Additional items/
// /.gitignore     -> Additional items/
// /icon.png       -> Additional items/
// /NuGet.config   -> Additional items/
// README.md       -> Additional items/
// Swallow.sln     -> solution containing *all* projects

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

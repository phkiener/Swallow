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

// generate              -> generate Swallow.sln and all PROJECT.sln
// add      NAME         -> create PROJECT (and all files) & regenerate
// publish  NAME VERSION -> dotnet publish PROJECT -o /publish/PROJECT

Console.WriteLine("Hello, World!");

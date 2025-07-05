using Swallow.Blazor.ExampleProject.StyleIsolationChecks;

if (args is ["--generate", var targetDirectory])
{
    await GenerateFiles.RunAsync(targetDirectory);
    return;
}

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
await app.RunAsync();

using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Swallow.Blazor.ExampleProject.StyleIsolationChecks;
using Swallow.Blazor.ExampleProject.Web;
using Swallow.Blazor.Reactive;

if (args is ["--generate", var targetDirectory])
{
    await GenerateFiles.RunAsync(targetDirectory);
    return;
}

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents();
builder.Services.AddReactiveRendering();

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

var app = builder.Build();

app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<_Root>();
app.MapRoutedComponents();
app.MapReactiveComponents();

await app.RunAsync();

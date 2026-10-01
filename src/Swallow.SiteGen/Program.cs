using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using Swallow.SiteGen;
using Swallow.SiteGen.Steps;

var builder = Host.CreateApplicationBuilder();
builder.Logging.AddFilter("Swallow.SiteGen", static l => l >= LogLevel.Information);
builder.Logging.AddFilter(static l => l >= LogLevel.Warning);
builder.Logging.AddSimpleConsole(static opt => opt.SingleLine = true);
builder.Services.AddHostedService<PrepareOutputDirectory>();
builder.Services.AddHostedService<GeneratePages>();
builder.Services.AddHostedService<CopyAssets>();
builder.Services.AddHostedService<ShutdownHost>();
builder.Services.AddRazorComponents();
builder.Services.AddSingleton<SourceCacheContext>();
builder.Services.AddSingleton<SourceRepository>(static _ => Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json"));

var buildOptions = new BuildOptions(targetPath: args[0]);
builder.Services.AddSingleton(buildOptions);
builder.Services.AddSingleton<IWebHostEnvironment>(static sp => sp.GetRequiredService<BuildOptions>());

StaticWebAssetsLoader.UseStaticWebAssets(buildOptions, builder.Configuration);

var host = builder.Build();
await host.RunAsync();

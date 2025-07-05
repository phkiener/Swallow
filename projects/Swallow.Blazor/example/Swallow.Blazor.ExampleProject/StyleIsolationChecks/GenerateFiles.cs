using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.HtmlRendering.Infrastructure;
using Microsoft.Extensions.Logging.Abstractions;

namespace Swallow.Blazor.ExampleProject.StyleIsolationChecks;

public static class GenerateFiles
{
    private const string ScopedCssLocationKey = "ScopedCssLocation";

    public static async Task RunAsync(string targetDirectory)
    {
        var scopedCssDirectory = GetScopedCssDirectory(typeof(Program).Assembly);
        if (scopedCssDirectory is null)
        {
            await Console.Error.WriteLineAsync($"Missing assembly metadata {ScopedCssLocationKey}.");
            return;
        }

        Directory.CreateDirectory(targetDirectory);

        await using var serviceProvider = new ServiceCollection().BuildServiceProvider();
        var renderer = new StaticHtmlRenderer(serviceProvider, new NullLoggerFactory());

        foreach (var type in typeof(Program).Assembly.GetExportedTypes().Where(static t => t.IsAssignableTo(typeof(ComponentBase))))
        {
            await GenerateFile(renderer, type, targetDirectory, scopedCssDirectory);
        }
    }

    private static string? GetScopedCssDirectory(Assembly assembly)
    {
        return assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(static a => a.Key is ScopedCssLocationKey)
            ?.Value;
    }

    private static async Task GenerateFile(StaticHtmlRenderer renderer, Type componentType, string targetDirectory, string cssDirectory)
    {
        await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var targetFile = Path.Combine(targetDirectory, $"{componentType.Name}.rendered");
            await using var fileStream = File.OpenWrite(targetFile);
            await using var writer = new StreamWriter(fileStream);

            var handle = renderer.BeginRenderingComponent(componentType, ParameterView.Empty);
            await handle.QuiescenceTask;

            await writer.WriteLineAsync(handle.ToHtmlString());

            var stylesPath = Path.Combine(cssDirectory, $"{componentType.Name}.razor.rz.scp.css");
            if (File.Exists(stylesPath))
            {
                var stylesheet = await File.ReadAllTextAsync(stylesPath);
                await writer.WriteLineAsync(stylesheet);
            }
        });
    }
}

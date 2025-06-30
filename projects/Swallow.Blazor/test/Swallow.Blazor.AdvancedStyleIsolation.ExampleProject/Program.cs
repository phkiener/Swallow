using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.HtmlRendering.Infrastructure;
using Microsoft.Extensions.Logging.Abstractions;

var scopedCssDirectory = Path.Combine(typeof(Program).Assembly.Location, "../../../../obj/Debug/net9.0/scopedcss");
var targetDirectory = args.ElementAtOrDefault(0) ?? "out";

#pragma warning disable ASP0000
await using var serviceProvider = new ServiceCollection().BuildServiceProvider();
var renderer = new StaticHtmlRenderer(serviceProvider, new NullLoggerFactory());

Directory.CreateDirectory(targetDirectory);

foreach (var type in typeof(Program).Assembly.GetExportedTypes().Where(static t => t.IsAssignableTo(typeof(ComponentBase))))
{
    await renderer.Dispatcher.InvokeAsync(async () =>
    {
        var targetFile = Path.Combine(targetDirectory, $"{type.Name}.rendered");
        await using var fileStream = File.OpenWrite(targetFile);
        await using var writer = new StreamWriter(fileStream);

        var handle = renderer.BeginRenderingComponent(type, ParameterView.Empty);
        await handle.QuiescenceTask;

        await writer.WriteLineAsync(handle.ToHtmlString());

        var stylesPath = Path.Combine(scopedCssDirectory, $"{type.Name}.razor.rz.scp.css");
        if (File.Exists(stylesPath))
        {
            var stylesheet = await File.ReadAllTextAsync(stylesPath);
            await writer.WriteLineAsync(stylesheet);
        }
    });
}



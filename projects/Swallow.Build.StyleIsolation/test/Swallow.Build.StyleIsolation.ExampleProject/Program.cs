using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.HtmlRendering.Infrastructure;
using Microsoft.Extensions.Logging.Abstractions;

#pragma warning disable ASP0000
await using var serviceProvider = new ServiceCollection().BuildServiceProvider();
var renderer = new StaticHtmlRenderer(serviceProvider, new NullLoggerFactory());

Directory.CreateDirectory("out");

foreach (var type in typeof(Program).Assembly.GetExportedTypes().Where(static t => t.IsAssignableTo(typeof(ComponentBase))))
{
    await renderer.Dispatcher.InvokeAsync(async () =>
    {
        var handle = renderer.BeginRenderingComponent(type, ParameterView.Empty);
        await handle.QuiescenceTask;

        await File.WriteAllTextAsync($"out/{type.Name}.razor.rendered", handle.ToHtmlString());

        var stylesPath = $"obj/Debug/net9.0/scopedcss/{type.Name}.razor.rz.scp.css";
        if (File.Exists(stylesPath))
        {
            await File.AppendAllTextAsync($"out/{type.Name}.razor.rendered", "\n");

            var stylesheet = await File.ReadAllTextAsync(stylesPath);
            await File.AppendAllTextAsync($"out/{type.Name}.razor.rendered", stylesheet);
        }
    });
}



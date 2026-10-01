using Microsoft.AspNetCore.Components.Web;
using Swallow.SiteGen.Pages;

namespace Swallow.SiteGen.Steps;

public sealed class GeneratePages(IServiceProvider serviceProvider, ILoggerFactory loggerFactory, BuildOptions buildOptions, ILogger<GeneratePages> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var renderer = new HtmlRenderer(serviceProvider, loggerFactory);

        var renderedPage = await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var component = await renderer.RenderComponentAsync<IndexPage>();
            return component.ToHtmlString();
        });

        logger.LogInformation("Generating {File}", "index.html");
        await File.WriteAllTextAsync(Path.Combine(buildOptions.TargetPath, "index.html"), renderedPage, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

namespace Swallow.SiteGen.Steps;

public sealed class PrepareOutputDirectory(BuildOptions buildOptions, ILogger<PrepareOutputDirectory> logger) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Preparing directory {Target}", Path.GetFullPath(buildOptions.TargetPath));
        Directory.CreateDirectory(buildOptions.TargetPath);

        foreach (var file in Directory.GetFiles(buildOptions.TargetPath))
        {
            File.Delete(file);
        }

        foreach (var directory in Directory.GetDirectories(buildOptions.TargetPath))
        {
            Directory.Delete(directory, recursive: true);
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

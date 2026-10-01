using Microsoft.Extensions.FileProviders;

namespace Swallow.SiteGen.Steps;

public class CopyAssets(BuildOptions buildOptions, ILogger<CopyAssets> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var files = DiscoverAssetFiles(buildOptions.WebRootFileProvider);
        foreach (var file in files)
        {
            var targetPath = Path.Combine(buildOptions.TargetPath, file.FilePath);
            logger.LogInformation("Copying asset {Path} to output", file.FilePath);

            var directory = Path.GetDirectoryName(targetPath);
            if (directory is not (null or ""))
            {
                Directory.CreateDirectory(directory);
            }

            await using var reader = file.CreateReadStream();
            await using var writer = File.OpenWrite(targetPath);

            await reader.CopyToAsync(writer, cancellationToken);
        }
    }

    private IEnumerable<DiscoveredAsset> DiscoverAssetFiles(IFileProvider fileProvider)
    {
        var directoryQueue = new Queue<string>();
        directoryQueue.Enqueue("");

        while (directoryQueue.TryDequeue(out var path))
        {
            var contents = fileProvider.GetDirectoryContents(path);
            foreach (var entry in contents)
            {
                if (entry.IsDirectory)
                {
                    directoryQueue.Enqueue(Path.Combine(path, entry.Name));
                }
                else
                {
                    yield return new DiscoveredAsset(path, entry);
                }
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private sealed class DiscoveredAsset(string path, IFileInfo fileInfo)
    {
        public string FilePath => Path.Combine(path, fileInfo.Name);
        public Stream CreateReadStream() => fileInfo.CreateReadStream();

        public override string ToString() => FilePath;
    }
}

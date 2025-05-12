using Swallow.TaskRunner.Abstractions;
using Swallow.TaskRunner.Serialization;

namespace Swallow.TaskRunner.Commands;

public sealed class RemoveTask : ICommand
{
    public async Task<int> RunAsync(ICommandContext console, string[] args)
    {
        var manifestFilePath = ManifestReader.FindManifestFile(console.CurrentDirectory);
        if (manifestFilePath is null)
        {
            throw new InvalidOperationException("No manifest file found.");
        }

        var manifest = await ManifestReader.ReadAsync(manifestFilePath, console.CancellationToken);
        manifest.RemoveTask(args[0]);

        await ManifestWriter.WriteAsync(manifest, manifestFilePath, console.CancellationToken);

        return 0;
    }
}

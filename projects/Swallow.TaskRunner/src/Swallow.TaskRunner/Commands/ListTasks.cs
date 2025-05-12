using Swallow.TaskRunner.Abstractions;
using Swallow.TaskRunner.Serialization;

namespace Swallow.TaskRunner.Commands;

public sealed class ListTasks : ICommand
{
    public async Task<int> RunAsync(ICommandContext console, string[] args)
    {
        var manifestFilePath = ManifestReader.FindManifestFile(console.CurrentDirectory);
        if (manifestFilePath is null)
        {
            throw new InvalidOperationException("No manifest file found.");
        }

        var manifest = await ManifestReader.ReadAsync(manifestFilePath, console.CancellationToken);

        foreach (var task in manifest.Tasks.Keys)
        {
            await console.Output.WriteLineAsync(task);
        }

        return 0;
    }
}

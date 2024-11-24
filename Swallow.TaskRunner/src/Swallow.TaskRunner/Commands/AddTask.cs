using Swallow.TaskRunner.Abstractions;
using Swallow.TaskRunner.Serialization;
using Swallow.TaskRunner.Tasks;

namespace Swallow.TaskRunner.Commands;

public sealed class AddTask : ICommand
{
    public async Task<int> RunAsync(ICommandContext console, string[] args)
    {
        var manifestFilePath = ManifestReader.FindManifestFile(console.CurrentDirectory);
        if (manifestFilePath is null)
        {
            throw new InvalidOperationException("No manifest file found.");
        }

        var manifest = await ManifestReader.ReadAsync(manifestFilePath, console.CancellationToken);

        var commands = args[1..].Select(static arg => new ShellTask(arg)).Cast<ITask>().ToList();
        manifest.AddTask(args[0], commands is [var single] ? single : new SequenceTask(commands));

        await ManifestWriter.WriteAsync(manifest, manifestFilePath, console.CancellationToken);

        return 0;
    }
}

namespace Swallow.TaskRunner.Commands;

public sealed class InvokeTask : ICommand
{
    public async Task<int> RunAsync(ICommandContext console, string[] args)
    {
        var manifestFilePath = Manifest.FindManifestFile(console);
        if (manifestFilePath is null)
        {
            throw new InvalidOperationException("No manifest file found.");
        }

        await using var fileStream = File.OpenRead(manifestFilePath);
        var manifest = await Manifest.ReadFromAsync(fileStream);

        var task = manifest.Tasks[string.Join(" ", args)];
        return await task.RunAsync(console);
    }
}

using System.Text.Json;
using Swallow.TaskRunner.Tasks;

namespace Swallow.TaskRunner;

public sealed class Manifest
{
    private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };
    private Dictionary<string, ITask> tasks = new() { ["test"] = new ShellTask("echo 'Hello World!'") };

    public int Version => 1;
    public IReadOnlyDictionary<string, ITask> Tasks => tasks.AsReadOnly();

    public static Manifest Create()
    {
        return new Manifest();
    }

    public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
    {
        await JsonSerializer.SerializeAsync(stream, this, SerializerOptions, cancellationToken);
    }

    public static string? FindManifestFile(ICommandContext context)
    {
        var currentDirectory = context.CurrentDirectory;
        var filePath = Path.Combine(currentDirectory, ".config", "dotnet-tasks.json");

        while (!File.Exists(filePath))
        {
            var parentDirectory = Directory.GetParent(currentDirectory);
            if (parentDirectory is null)
            {
                return null;
            }

            currentDirectory = parentDirectory.FullName;
        }

        return filePath;
    }

    public static async Task<Manifest> ReadFromAsync(Stream stream, CancellationToken cancellationToken)
    {
        var manifest = await JsonSerializer.DeserializeAsync<Manifest>(stream, cancellationToken: cancellationToken);
        return manifest ?? throw new InvalidOperationException("Failed to deserialize manifest.");
    }
}

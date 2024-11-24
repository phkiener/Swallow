using System.Text.Json;
using Swallow.TaskRunner.Abstractions;
using Swallow.TaskRunner.Tasks;

namespace Swallow.TaskRunner.Serialization;

public static class ManifestReader
{
    private static readonly JsonDocumentOptions readerOptions = new() { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true };

    private const string VersionProperty = "Version";
    private static readonly string[] reservedProperties = [VersionProperty];

    public static string? FindManifestFile(string directory)
    {
        var currentDirectory = directory;
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

    public static async Task<Manifest> ReadAsync(Stream stream, CancellationToken cancellationToken)
    {
        var jsonDocument = await JsonDocument.ParseAsync(stream, readerOptions, cancellationToken);
        var root = jsonDocument.RootElement;

        if (!root.IsObject())
        {
            throw new InvalidDataException($"Invalid manifest format - expected object but got {jsonDocument.RootElement.ValueKind}.");
        }

        if (!root.HasProperty(propertyName: VersionProperty, expected: "1", actual: out var actualVersion))
        {
            throw new InvalidDataException($"Manifest has unknown version '{actualVersion}'.");
        }

        var manifest = Manifest.Create();

        foreach (var property in root.EnumerateObject().ExceptBy(reservedProperties, static prop => prop.Name))
        {
            var task = ReadTask(property.Value);
            manifest.AddTask(property.Name, task);
        }

        return manifest;
    }

    private static ITask ReadTask(JsonElement value)
    {
        if (value.IsArray())
        {
            var subtasks = value.EnumerateArray().Select(ReadTask);
            return new SequenceTask(subtasks);
        }

        if (value.IsObject())
        {
            throw new InvalidOperationException("Unknown task type.");
        }

        var command = value.GetRawText();
        return new ShellTask(command);
    }
}

file static class JsonExtensions
{
    public static bool IsObject(this JsonElement element) => element.ValueKind == JsonValueKind.Object;

    public static bool IsArray(this JsonElement element) => element.ValueKind == JsonValueKind.Array;

    public static bool HasProperty(this JsonElement element, string propertyName, string expected, out string actual)
    {
        actual = element.GetProperty(propertyName).GetRawText();

        return expected == actual;
    }
}

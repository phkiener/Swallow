using System.Text.Encodings.Web;
using System.Text.Json;
using Swallow.TaskRunner.Abstractions;
using Swallow.TaskRunner.Tasks;

namespace Swallow.TaskRunner.Serialization;

public static class ManifestWriter
{
    private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };


    public static async Task WriteAsync(Manifest manifest, string path, CancellationToken cancellationToken)
    {
        await using var fileStream = File.Open(path, FileMode.Create);
        await WriteAsync(manifest, fileStream, cancellationToken);
    }

    public static async Task WriteAsync(Manifest manifest, Stream stream, CancellationToken cancellationToken)
    {
        var serializedData = new Dictionary<string, object> { ["Version"] = manifest.Version };
        foreach (var task in manifest.Tasks)
        {
            serializedData.Add(task.Key, Transform(task.Value));
        }

        await JsonSerializer.SerializeAsync(stream, serializedData, SerializerOptions, cancellationToken: cancellationToken);
    }

    private static object Transform(ITask task)
    {
        return task switch
        {
            ShellTask shell => shell.Commandline,
            SequenceTask sequence => sequence.Tasks.Select(Transform).ToList(),
            _ => throw new ArgumentException($"Unknown task type {task.GetType()}.")
        };
    }
}


using System.Text.Json;

namespace Swallow.TaskRunner;

public sealed class Manifest
{
    private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };
    public int Version => 1;

    public static Manifest Create()
    {
        return new Manifest();
    }

    public static async Task<Manifest> ReadFromAsync(Stream stream)
    {
        var manifest = await JsonSerializer.DeserializeAsync<Manifest>(stream);
        return manifest ?? throw new InvalidOperationException("Failed to deserialize manifest.");
    }

    public async Task WriteToAsync(Stream stream)
    {
        await JsonSerializer.SerializeAsync(stream, this, SerializerOptions);
    }

}

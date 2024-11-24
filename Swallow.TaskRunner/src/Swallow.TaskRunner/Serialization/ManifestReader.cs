using System.Text.Json;

namespace Swallow.TaskRunner.Serialization;

public static class ManifestReader
{
    private static readonly JsonDocumentOptions readerOptions = new() { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true };

    private const string VersionProperty = "Version";
    private static readonly string[] reservedProperties = [VersionProperty];

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

        foreach (var property in jsonDocument.RootElement.EnumerateObject().ExceptBy(reservedProperties, static prop => prop.Name))
        {
            if (property.Value.IsArray())
            {
                manifest.AddShellSequence(property.Name, property.Value.EnumerateArray().Select(static element => element.GetRawText()));
            }
            else
            {
                manifest.AddShellTask(property.Name, property.Value.GetRawText());
            }
        }

        return manifest;
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

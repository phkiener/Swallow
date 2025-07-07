using System.IO.Compression;
using System.Text.Json;
using Swallow.Blazor.Reactive.Abstractions.State;

namespace Swallow.Blazor.Reactive.State;

/// <summary>
/// A simple <see cref="IStateSerializer"/> that uses <see cref="object.ToString"/> for primitive
/// types, URL encoding for strings and a brotli-encoded JSON serialization in base64 for all other types.
/// </summary>
public sealed class DefaultSerializer : IStateSerializer
{
    private const string BrotliPrefix = "br:";

    /// <inheritdoc />
    public string? Serialize(object? data)
    {
        if (data is null)
        {
            return null;
        }

        if (data is string text)
        {
            return Uri.EscapeDataString(text);
        }

        if (data.GetType().IsPrimitive)
        {
            return data.ToString();
        }


        using var resultStream = new MemoryStream();
        using var brotliStream = new BrotliStream(resultStream, CompressionMode.Compress);
        JsonSerializer.Serialize(brotliStream, data);
        brotliStream.Flush();

        return "br:" + Convert.ToBase64String(resultStream.ToArray());
    }

    /// <inheritdoc />
    public object? Deserialize(Type targetType, string? text)
    {
        if (targetType == typeof(string) && text is not null)
        {
            return Uri.UnescapeDataString(text);
        }

        if (text is null or "")
        {
            return null;
        }

        if (text.StartsWith(BrotliPrefix))
        {
            using var inputStream = new MemoryStream(Convert.FromBase64String(text[BrotliPrefix.Length..]));
            using var brotliStream = new BrotliStream(inputStream, CompressionMode.Decompress);

            return JsonSerializer.Deserialize(brotliStream, targetType);
        }

        return Convert.ChangeType(text, targetType);
    }
}

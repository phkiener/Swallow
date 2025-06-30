using System.CodeDom.Compiler;

namespace Swallow.Blazor.StaticAssetPaths;

/// <summary>
/// Helper to write the generated source file from an <see cref="AssetMap"/>.
/// </summary>
public static class AssetMapWriter
{
    private const string WwwrootPrefix = "wwwroot/";

    /// <summary>
    /// Write the given asset map as source code.
    /// </summary>
    /// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
    /// <param name="assets">The <see cref="AssetMap"/> to write as source code.</param>
    public static void WriteTo(TextWriter writer, AssetMap assets)
    {
        using var indentedWriter = new IndentedTextWriter(writer);
        WriteNode(indentedWriter, assets.RootNode());
    }

    private static void WriteNode(IndentedTextWriter writer, AssetMap.Node node)
    {
        writer.WriteLine($"public static class {node.Name}");
        writer.WriteLine("{");

        writer.Indent++;
        foreach (var asset in node.Assets.Where(IncludeAsset))
        {
            var filePath = asset.Path;
            if (filePath.StartsWith(WwwrootPrefix))
            {
                filePath = filePath.Substring(WwwrootPrefix.Length);
            }

            writer.WriteLine("/// <summary>");
            writer.WriteLine($"/// Points to <c>{asset.Path}</c>");
            writer.WriteLine("/// </summary>");
            writer.WriteLine($"public static readonly string {asset.NameSegments.Last()} = \"{filePath}\";");
            writer.WriteLine();
        }

        foreach (var child in node.Nodes)
        {
            WriteNode(writer, child);
        }

        writer.Indent--;
        writer.WriteLine("}");
        writer.WriteLine();
    }

    private static bool IncludeAsset(DefinedAsset asset)
    {
        return Path.GetExtension(asset.Path) is not (".razor" or ".cshtml");
    }
}

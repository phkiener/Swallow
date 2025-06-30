using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Swallow.Blazor.StaticAssetPaths;

[Generator]
public sealed class AssetPathGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var sourceData = context.AdditionalTextsProvider.Collect()
            .Combine(context.AnalyzerConfigOptionsProvider);
        context.RegisterSourceOutput(sourceData, GeneratePathFile);
    }

    private static void GeneratePathFile(
        SourceProductionContext context,
        (ImmutableArray<AdditionalText> Left, AnalyzerConfigOptionsProvider Right) sourceData)
    {
        if (sourceData.Left.IsDefaultOrEmpty)
        {
            return;
        }

        var textBuilder = new StringBuilder();
        var rootNamespace = sourceData.Right.GlobalOptions.TryGetValue("build_property.rootnamespace", out var a) ? a : "Assets";

        textBuilder.AppendLine($"namespace {rootNamespace};");
        textBuilder.AppendLine();
        textBuilder.AppendLine("public static class AssetPaths");
        textBuilder.AppendLine("{");

        var projectDirectory = sourceData.Right.GlobalOptions.TryGetValue("build_property.projectdir", out var b) ? b : "/";
        var map = new AssetMap(FindAssets(projectDirectory, sourceData.Left));
        foreach (var asset in map.EnumerateNodes())
        {
            WriteNode(textBuilder, asset);
        }

        textBuilder.AppendLine("}");

        context.AddSource("StaticAssets.g.cs", textBuilder.ToString());
    }

    private static IEnumerable<DefinedAsset> FindAssets(string projectDirectory, IEnumerable<AdditionalText> files)
    {
        foreach (var file in files)
        {
            if (!file.Path.StartsWith(projectDirectory))
            {
                continue;
            }

            var extension = Path.GetExtension(file.Path);
            if (extension is ".razor")
            {
                continue;
            }

            var relativePath = file.Path.Substring(projectDirectory.Length);
            var fileName = Path.GetFileNameWithoutExtension(file.Path).Replace('.', '_');
            var segments = Path.GetDirectoryName(relativePath)?.Split(['/'], StringSplitOptions.RemoveEmptyEntries) ?? [];

            yield return new DefinedAsset(
                relativePath,
                [
                    ToSafeIdentifier(ToPascalCase(extension.Substring(1))),
                    ..segments.Select(ToPascalCase).Select(ToSafeIdentifier),
                    ToSafeIdentifier(ToPascalCase(fileName))
                ]);
        }
    }

    private static void WriteNode(StringBuilder builder, AssetMap.Node node, int indent = 4)
    {
        var padding = new string(' ', indent);
        builder.AppendLine($"{padding}public static class {node.Name}");
        builder.AppendLine($"{padding}{{");
        foreach (var asset in node.Assets)
        {
            builder.AppendLine($"{padding}    /// <summary>");
            builder.AppendLine($"{padding}    /// Path for <c>{asset.Path}</c>");
            builder.AppendLine($"{padding}    /// </summary>");
            builder.AppendLine($"{padding}    public static readonly string {asset.NameSegments.Last()} = \"{asset.Path}\";");
            builder.AppendLine();
        }

        foreach (var child in node.Nodes)
        {
            WriteNode(builder, child, indent + 4);
            builder.AppendLine();
        }

        builder.AppendLine($"{padding}}}");
    }

    private static string ToPascalCase(string text)
    {
        if (text.Length < 1)
        {
            return text;
        }

        return char.ToUpperInvariant(text[0]) + text.Substring(1);
    }

    private static string ToSafeIdentifier(string text)
    {
        return text.Replace('.', '_').Replace('-', '_');
    }
}

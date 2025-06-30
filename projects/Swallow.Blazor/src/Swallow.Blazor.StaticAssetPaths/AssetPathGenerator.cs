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

        try
        {
            var textBuilder = new StringBuilder();
            var rootNamespace = sourceData.Right.GlobalOptions.TryGetValue("build_property.rootnamespace", out var a) ? a : "Assets";

            textBuilder.AppendLine($"namespace {rootNamespace};");
            textBuilder.AppendLine();

            var projectDirectory = sourceData.Right.GlobalOptions.TryGetValue("build_property.projectdir", out var b) ? b : "/";
            var map = new AssetMap(FindAssets(projectDirectory, sourceData.Left));
            WriteNode(textBuilder, map.RootNode());

            context.AddSource("StaticAssets.g.cs", textBuilder.ToString());
        }
        catch (Exception e)
        {
            context.AddSource("StaticAssets.g.text", e.ToString());
        }
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
            var fileName = Path.GetFileName(file.Path);
            var segments = Path.GetDirectoryName(relativePath)?.Split(['/'], StringSplitOptions.RemoveEmptyEntries) ?? [];

            yield return new DefinedAsset(
                relativePath,
                [
                    ..segments.Select(ToPascalCase),
                    ToPascalCase(fileName)
                ]);
        }
    }

    private static void WriteNode(StringBuilder builder, AssetMap.Node node, int indent = 0)
    {
        var padding = new string(' ', indent);
        builder.AppendLine($"{padding}public static class {node.Name}");
        builder.AppendLine($"{padding}{{");
        foreach (var asset in node.Assets)
        {
            builder.AppendLine($"{padding}    /// <summary>");
            builder.AppendLine($"{padding}    /// Points to <c>{asset.Path}</c>");
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

        var words = text.Split(['.', '_', '-'], StringSplitOptions.RemoveEmptyEntries);
        return string.Join("", words.Select(static w => char.ToUpperInvariant(w[0]) + w.Substring(1)));
    }
}

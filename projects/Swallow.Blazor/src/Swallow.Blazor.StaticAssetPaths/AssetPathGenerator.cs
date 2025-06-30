using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Swallow.Blazor.StaticAssetPaths;

/// <summary>
/// A source generator to produce a file containing constants for each found web asset.
/// </summary>
/// <remarks>
/// All web assets to be included need to be added to the <c>AdditionalFiles</c> item.
/// </remarks>
[Generator]
public sealed class AssetPathGenerator : IIncrementalGenerator
{
    /// <inheritdoc />
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
            var writer = new StringWriter();
            var rootNamespace = sourceData.Right.GlobalOptions.TryGetValue("build_property.rootnamespace", out var a) ? a : "Assets";
            writer.WriteLine($"namespace {rootNamespace};");
            writer.WriteLine();

            var projectDirectory = sourceData.Right.GlobalOptions.TryGetValue("build_property.projectdir", out var b) ? b : "/";
            var allAssets = FindAssets(projectDirectory, sourceData.Left);
            var assetMap = new AssetMap(allAssets);

            AssetMapWriter.WriteTo(writer, assetMap);

            context.AddSource("StaticAssets.g.cs", writer.ToString());
        }
        catch (Exception e)
        {
            context.AddSource("StaticAssets.g.crash.text", e.ToString());
        }
    }

    private static IEnumerable<DefinedAsset> FindAssets(string projectDirectory, IEnumerable<AdditionalText> files)
    {
        return files
            .Where(f => f.Path.StartsWith(projectDirectory))
            .Select(f => f.Path.Substring(projectDirectory.Length))
            .Select(DefinedAsset.For);
    }
}

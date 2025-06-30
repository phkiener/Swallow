using System.Collections.Immutable;

namespace Swallow.Blazor.StaticAssetPaths;

/// <summary>
/// A web asset.
/// </summary>
public readonly struct DefinedAsset
{
    /// <summary>
    /// Construct an (invalid) web-asset at a default location.
    /// </summary>
    public DefinedAsset()
    {
        Path = "/";
        NameSegments = [];
    }

    /// <summary>
    /// Construct an asset for the given file.
    /// </summary>
    /// <param name="path">Relative path of the asset inside the project.</param>
    /// <param name="nameSegments">A list of all parts (i.e. directory and file names) contained in the path.</param>
    public DefinedAsset(string path, IEnumerable<string> nameSegments)
    {
        Path = path;
        NameSegments = [..nameSegments];
    }

    /// <summary>
    /// Relative path of the asset inside the project.
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// A list of all parts (i.e. directory and file names) contained in the path.
    /// </summary>
    public ImmutableArray<string> NameSegments { get; }

    /// <summary>
    /// Builds a <see cref="DefinedAsset"/> for the given relative path.
    /// </summary>
    /// <param name="relativePath">The path for which to build an asset.</param>
    /// <returns>The built asset.</returns>
    public static DefinedAsset For(string relativePath)
    {
        var correctedPath = relativePath.Replace('\\', '/');
        var segments = correctedPath.Split(['/'], StringSplitOptions.RemoveEmptyEntries);

        return new DefinedAsset(correctedPath, [..segments.Select(ToPascalCase)]);
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

using System.Collections.Immutable;

namespace Swallow.Blazor.StaticAssetPaths;

public readonly struct DefinedAsset
{
    public DefinedAsset()
    {
        Path = "/";
        NameSegments = [];
    }

    public DefinedAsset(string path, IEnumerable<string> nameSegments)
    {
        Path = path;
        NameSegments = [..nameSegments];
    }

    public string Path { get; }
    public ImmutableArray<string> NameSegments { get; }
}

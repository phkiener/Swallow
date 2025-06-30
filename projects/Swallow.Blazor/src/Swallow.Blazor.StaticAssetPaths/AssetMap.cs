using System.Collections.Immutable;

namespace Swallow.Blazor.StaticAssetPaths;

public sealed class AssetMap(IEnumerable<DefinedAsset> assets)
{
    private readonly ImmutableArray<DefinedAsset> assets = [..assets.OrderBy(static a => a.Path)];

    public sealed class Node(string name, IEnumerable<DefinedAsset> assets, IEnumerable<Node> nodes)
    {
        public string Name { get; } = name;
        public ImmutableArray<DefinedAsset> Assets { get; } = [..assets.OrderBy(static a => a.Path)];
        public ImmutableArray<Node> Nodes { get; } = [..nodes.OrderBy(static a => a.Name)];
    }

    public IEnumerable<Node> EnumerateNodes()
    {
        return assets
            .GroupBy(static asset => asset.NameSegments[0])
            .Select(static segment => BuildNode(segment, 1));
    }

    private static Node BuildNode(IGrouping<string, DefinedAsset> group, int nextIndex)
    {
        var assets = group.Where(asset => asset.NameSegments.Length <= nextIndex + 1);

        var groups = group
            .Where(asset => asset.NameSegments.Length > nextIndex + 1)
            .GroupBy(asset => asset.NameSegments[nextIndex])
            .Select(g => BuildNode(g, nextIndex + 1));

        return new Node(group.Key, assets, groups);
    }
}

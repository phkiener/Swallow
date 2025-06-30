using System.Collections;
using System.Collections.Immutable;

namespace Swallow.Blazor.StaticAssetPaths;

/// <summary>
/// A map of assets, implemented as trie over their paths.
/// </summary>
/// <param name="assets">A list of all contained <see cref="DefinedAsset"/>s.</param>
public sealed class AssetMap(IEnumerable<DefinedAsset> assets)
{
    private readonly ImmutableArray<DefinedAsset> assets = [..assets.OrderBy(static a => a.Path)];

    /// <summary>
    /// A node in the asset map.
    /// </summary>
    /// <param name="name">Name of the current segment.</param>
    /// <param name="assets">All assets contained directly in this segment.</param>
    /// <param name="nodes">A list of child nodes, each with their own name.</param>
    public sealed class Node(string name, IEnumerable<DefinedAsset> assets, IEnumerable<Node> nodes)
    {
        /// <summary>
        /// Name of the current segment.
        /// </summary>
        public string Name { get; } = name;

        /// <summary>
        /// All assets contained directly in this segment.
        /// </summary>
        public ImmutableArray<DefinedAsset> Assets { get; } = [..assets.OrderBy(static a => a.Path)];

        /// <summary>
        /// A list of child nodes, each with their own name.
        /// </summary>
        public ImmutableArray<Node> Nodes { get; } = [..nodes.OrderBy(static a => a.Name)];
    }

    /// <summary>
    /// Return the top-level node of this asset map.
    /// </summary>
    /// <returns>The top-level <see cref="Node"/>, which has a fixed <see cref="Node.Name"/>.</returns>
    public Node RootNode()
    {
        return BuildNode(new Group<string, DefinedAsset>("AssetPaths", assets), 0);
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

    private sealed class Group<TKey, TValue>(TKey name, IEnumerable<TValue> items) : IGrouping<TKey, TValue>
    {
        public IEnumerator<TValue> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public TKey Key { get; } = name;
    }
}

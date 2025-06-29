using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Swallow.Build.StyleIsolation;

/// <summary>
/// A build task that allows a given component to use both its own styles and the styles from another component.
/// </summary>
public sealed class AppendCssScope : Task
{
    private const string FromMetadata = "From";
    private const string CssScopeMetadata = "CssScope";

    /// <summary>
    /// All <c>@(InheritStyles)</c> items whose CSS scope to adjust.
    /// </summary>
    [Required]
    public ITaskItem[] Items { get; set; }

    /// <summary>
    /// All existing stylesheets; usually <c>@(RazorComponent)</c>.
    /// </summary>
    [Required]
    public ITaskItem[] Components { get; set; }

    /// <summary>
    /// All relevant items from <see cref="Components"/> that belong to <see cref="Items"/> with their CSS scopes replaced.
    /// </summary>
    [Output]
    public ITaskItem[] AdjustedComponents { get; private set; }

    /// <inheritdoc />
    public override bool Execute()
    {
        if (DetectLoops(Items))
        {
            Log.LogError("Loops detected while enumerating items.");
            return false;
        }

        var adjustedComponents = new List<ITaskItem>();

        foreach (var item in Items)
        {
            var inheritedComponent = Components.SingleOrDefault(s => s.ItemSpec == item.GetMetadata(FromMetadata));
            if (inheritedComponent is null)
            {
                Log.LogWarning("Component {0} cannot append styles from {1}: Component not found.", item.ItemSpec, item.GetMetadata(FromMetadata));
                continue;
            }

            var scope = inheritedComponent.GetMetadata(CssScopeMetadata);
            if (string.IsNullOrEmpty(scope))
            {
                Log.LogWarning("Component {0} cannot append styles from {1}: Component does not have a CSS scope.", item.ItemSpec, inheritedComponent.ItemSpec);
                continue;
            }

            var definedComponent = Components.SingleOrDefault(s => s.ItemSpec == item.ItemSpec);
            if (definedComponent is not null)
            {
                var originalScope = definedComponent.GetMetadata(CssScopeMetadata);
                definedComponent.SetMetadata(CssScopeMetadata, $"{originalScope} {scope}".Trim());
                adjustedComponents.Add(definedComponent);
            }
        }

        AdjustedComponents = adjustedComponents.ToArray();

        return true;
    }

    private static bool DetectLoops(ITaskItem[] items)
    {
        var allItems = new HashSet<string>(items.Select(static i => i.ItemSpec));
        var allInherits = new HashSet<string>(items.Select(static i => i.GetMetadata(FromMetadata)));

        return allItems.Overlaps(allInherits);
    }
}

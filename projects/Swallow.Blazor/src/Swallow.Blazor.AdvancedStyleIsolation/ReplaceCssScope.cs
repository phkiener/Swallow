using Microsoft.Build.Framework;
using Task = Microsoft.Build.Utilities.Task;

namespace Swallow.Blazor.AdvancedStyleIsolation;

/// <summary>
/// A build task that replaces the CSS scope of given components and their stylesheets.
/// </summary>
public sealed class ReplaceCssScope : Task
{
    private const string FromMetadata = "From";
    private const string CssScopeMetadata = "CssScope";

    /// <summary>
    /// All <c>@(InheritStyles)</c> items whose CSS scope to adjust.
    /// </summary>
    [Required]
    public ITaskItem[] Items { get; set; } = [];

    /// <summary>
    /// All existing stylesheets; usually <c>@(RazorComponent)</c>.
    /// </summary>
    [Required]
    public ITaskItem[] Components { get; set; } = [];

    /// <summary>
    /// All existing stylesheets; usually <c>@(_ScopedCss)</c>.
    /// </summary>
    [Required]
    public ITaskItem[] Styles { get; set; } = [];

    /// <summary>
    /// All relevant items from <see cref="Components"/> that belong to <see cref="Items"/> with their CSS scopes replaced.
    /// </summary>
    [Output]
    public ITaskItem[] AdjustedComponents { get; private set; } = [];

    /// <summary>
    /// All relevant items from <see cref="Styles"/> that belong to <see cref="Items"/> with their CSS scopes replaced.
    /// </summary>
    [Output]
    public ITaskItem[] AdjustedStyles { get; private set; } = [];

    /// <inheritdoc />
    public override bool Execute()
    {
        if (DetectLoops(Items))
        {
            Log.LogError("Loops detected while enumerating items.");
            return false;
        }

        var adjustedComponents = new List<ITaskItem>();
        var adjustedStyles = new List<ITaskItem>();

        foreach (var item in Items)
        {
            var inheritedComponent = Components.SingleOrDefault(s => s.ItemSpec == item.GetMetadata(FromMetadata));
            if (inheritedComponent is null)
            {
                Log.LogWarning("Component {0} cannot inherit styles from {1}: Component not found.", item.ItemSpec, item.GetMetadata(FromMetadata));
                continue;
            }

            var scope = inheritedComponent.GetMetadata(CssScopeMetadata);
            if (string.IsNullOrEmpty(scope))
            {
                Log.LogWarning("Component {0} cannot inherit styles from {1}: Component does not have a CSS scope.", item.ItemSpec, inheritedComponent.ItemSpec);
                continue;
            }

            var definedComponent = Components.SingleOrDefault(s => s.ItemSpec == item.ItemSpec);
            if (definedComponent is not null)
            {
                definedComponent.SetMetadata(CssScopeMetadata, scope);
                adjustedComponents.Add(definedComponent);
            }

            var definedStyle = Styles.SingleOrDefault(s => s.ItemSpec == item.ItemSpec + ".css");
            if (definedStyle is not null)
            {
                definedStyle.SetMetadata(CssScopeMetadata, scope);
                adjustedStyles.Add(definedStyle);
            }
        }

        AdjustedComponents = adjustedComponents.ToArray();
        AdjustedStyles = adjustedStyles.ToArray();

        return true;
    }

    private static bool DetectLoops(ITaskItem[] items)
    {
        var allItems = new HashSet<string>(items.Select(static i => i.ItemSpec));
        var allInherits = new HashSet<string>(items.Select(static i => i.GetMetadata(FromMetadata)));

        return allItems.Overlaps(allInherits);
    }
}

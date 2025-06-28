using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Swallow.Build.StyleIsolation;

/// <summary>
/// A build task that replaces the CSS scope of given components and their stylesheets.
/// </summary>
public sealed class ReplaceCssScope : Task
{
    private const string InheritMetadata = "Inherit";
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
    public ITaskItem[] RazorComponents { get; set; }

    /// <summary>
    /// All existing stylesheets; usually <c>@(_ScopedCss)</c>.
    /// </summary>
    [Required]
    public ITaskItem[] Styles { get; set; }

    /// <summary>
    /// All relevant items from <see cref="RazorComponents"/> that belong to <see cref="Items"/> with their CSS scopes replaced.
    /// </summary>
    [Output]
    public ITaskItem[] AdjustedComponents { get; set; }

    /// <summary>
    /// All relevant items from <see cref="Styles"/> that belong to <see cref="Items"/> with their CSS scopes replaced.
    /// </summary>
    [Output]
    public ITaskItem[] AdjustedStyles { get; set; }

    /// <inheritdoc />
    public override bool Execute()
    {
        var adjustedComponents = new List<ITaskItem>();
        var adjustedStyles = new List<ITaskItem>();

        foreach (var item in Items)
        {
            var inheritedComponent = RazorComponents.SingleOrDefault(s => s.ItemSpec == item.GetMetadata(InheritMetadata));
            if (inheritedComponent is null)
            {
                Log.LogWarning("Component {0} cannot inherit styles from {1}: Component not found.", item.ItemSpec, item.GetMetadata(InheritMetadata));
                continue;
            }

            var definedComponent = RazorComponents.SingleOrDefault(s => s.ItemSpec == item.ItemSpec);
            if (definedComponent is not null)
            {
                definedComponent.SetMetadata(CssScopeMetadata, inheritedComponent.GetMetadata(CssScopeMetadata));
                adjustedComponents.Add(definedComponent);
            }

            var definedStyle = Styles.SingleOrDefault(s => s.ItemSpec == item.ItemSpec + ".css");
            if (definedStyle is not null)
            {
                definedStyle.SetMetadata(CssScopeMetadata, inheritedComponent.GetMetadata(CssScopeMetadata));
                adjustedStyles.Add(definedStyle);
            }
        }

        AdjustedComponents = adjustedComponents.ToArray();
        AdjustedStyles = adjustedStyles.ToArray();

        return true;
    }
}

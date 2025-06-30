using System.Collections;
using Microsoft.Build.Framework;

namespace Swallow.Blazor.AdvancedStyleIsolation.Framework;

public abstract class TaskItem(string Identity) : ITaskItem
{
    private readonly Dictionary<string, string> metadata = new();

    public string ItemSpec { get; set; } = Identity;
    public ICollection MetadataNames => metadata.Keys;
    public int MetadataCount => metadata.Count;

    public string GetMetadata(string metadataName)
    {
        return metadata.GetValueOrDefault(metadataName);
    }

    public void SetMetadata(string metadataName, string metadataValue)
    {
        if (metadataValue is null)
        {
            metadata.Remove(metadataName);
        }
        else
        {
            metadata[metadataName] = metadataValue;
        }
    }

    public void RemoveMetadata(string metadataName)
    {
        metadata.Remove(metadataName);
    }

    public void CopyMetadataTo(ITaskItem destinationItem)
    {
        foreach (var (key, value) in metadata)
        {
            destinationItem.SetMetadata(key, value);
        }
    }

    public IDictionary CloneCustomMetadata()
    {
        return new Dictionary<string, string>(metadata);
    }
}

public sealed class InheritStyleItem : TaskItem
{
    public InheritStyleItem(string Path, string From) : base(Path)
    {
        SetMetadata("From", From);
    }
}

public sealed class AppendStyleItem : TaskItem
{
    public AppendStyleItem(string Path, string From) : base(Path)
    {
        SetMetadata("From", From);
    }
}

public sealed class RazorComponentItem : TaskItem
{
    public RazorComponentItem(string Path, string CssScope) : base(Path)
    {
        SetMetadata("CssScope", CssScope);
    }
}
public sealed class ScopedCssItem : TaskItem
{
    public ScopedCssItem(string Path, string CssScope) : base(Path)
    {
        SetMetadata("CssScope", CssScope);
    }
}


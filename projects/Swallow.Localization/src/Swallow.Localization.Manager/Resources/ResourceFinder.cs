using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;

namespace Swallow.Localization.Manager.Resources;

public static partial class ResourceFinder
{
    public static async IAsyncEnumerable<ResourceKey> FindResourcesAsync(Project project)
    {
        var projectPath = Path.GetDirectoryName(project.FilePath);
        if (projectPath is null)
        {
            yield break;
        }

        var resourceFiles = Directory.GetFiles(projectPath, "*.resx", SearchOption.AllDirectories);
        foreach (var file in resourceFiles)
        {
            var scope = ResourceNameRegex().Match(Path.GetFileName(file)).Groups["Scope"].Value;

            await using var fileStream = File.OpenRead(file);
            var xmlDocument = await XDocument.LoadAsync(fileStream, LoadOptions.PreserveWhitespace, CancellationToken.None);

            var keys = xmlDocument.DescendantNodes().OfType<XElement>()
                .Where(static el => el.Name.LocalName == "data" && el.Attributes().Any(static att => att.Name.LocalName is "name"))
                .Select(static n => n.Attribute("name")!.Value)
                .ToHashSet();

            foreach (var key in keys)
            {
                yield return new ResourceKey(project.Name, scope, key);
            }
        }

    }

    [GeneratedRegex(@"^(?<Scope>[a-zA-Z\-\.]*)(\.([a-zA-Z0-9\-]+))?\.resx$")]
    private static partial Regex ResourceNameRegex();

}

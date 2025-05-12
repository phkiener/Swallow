using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Swallow.Manager.Commands;

public static class Generate
{
    public static int Run(string directory)
    {
        var projects = Directory.GetDirectories(directory, "Swallow.*", SearchOption.TopDirectoryOnly)
            .Select(GetProject)
            .OrderBy(static p => p.Name)
            .ToList();

        foreach (var project in projects)
        {
            var projectPath = Path.Combine(directory, project.Name);

            var additionalItems = GetFileFolder(project.AdditionalItems, "Additional items", projectPath);
            var exampleProjects = GetProjectFolder(project.ExampleProjects, "Example", projectPath);
            var sourceProjects = GetProjectFolder(project.SourceProjects, "Library", projectPath);
            var testProjects = GetProjectFolder(project.TestProjects, "Test", projectPath);

            var solutionContent = new XDocument(new XElement("Solution", additionalItems, exampleProjects, sourceProjects, testProjects));
            WriteXml(solutionContent, Path.Combine(projectPath, $"{project.Name}.slnx"));
        }

        var projectFolders = new List<XElement?>
        {
            new XElement("Folder",
                new XAttribute("Name", "/Additional items/"),
                new XElement("File", new XAttribute("Path", ".editorconfig")),
                new XElement("File", new XAttribute("Path", ".gitattributes")),
                new XElement("File", new XAttribute("Path", ".gitignore")),
                new XElement("File", new XAttribute("Path", "README.md")),
                new XElement("File", new XAttribute("Path", "NuGet.config")),
                new XElement("File", new XAttribute("Path", "icon.png"))),
            new XElement("Folder",
                new XAttribute("Name", "/Additional items/Assets/"),
                new XElement("File", new XAttribute("Path", "assets/swallow.svg")),
                new XElement("File", new XAttribute("Path", "assets/swallow-icon-rect.svg")),
                new XElement("File", new XAttribute("Path", "assets/swallow-icon-round.svg"))),
            new XElement("Folder",
                new XAttribute("Name", "/Additional items/Build/"),
                new XElement("File", new XAttribute("Path", "build/Build-Package.ps1")),
                new XElement("File", new XAttribute("Path", "build/build-package.sh")),
                new XElement("File", new XAttribute("Path", "build/Publish.props"))),
            new XElement("Folder",
                new XAttribute("Name", "/Tooling/"),
                new XElement("Project", new XAttribute("Path", "tooling/Swallow.Manager/Swallow.Manager.csproj"))),
        };

        foreach (var project in projects)
        {
            var folder = new XElement("Folder", new XAttribute("Name", $"/{project.Name}/"));
            var additionalItems = GetFileFolder(project.AdditionalItems, $"{project.Name}/Additional items", directory);
            var exampleProjects = GetProjectFolder(project.ExampleProjects, $"{project.Name}/Example", directory);
            var sourceProjects = GetProjectFolder(project.SourceProjects, $"{project.Name}/Library", directory);
            var testProjects = GetProjectFolder(project.TestProjects, $"{project.Name}/Test", directory);

            projectFolders.AddRange([folder, additionalItems, exampleProjects, sourceProjects, testProjects]);
        }

        var mainSolution = new XDocument(new XElement("Solution", projectFolders));
        WriteXml(mainSolution, Path.Combine(directory, "Swallow.slnx"));

        return 0;
    }

    private static Project GetProject(string path)
    {
        return new Project(
            Name: Path.GetFileName(path),
            AdditionalItems: Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).Where(IsAdditionalItem).ToArray(),
            SourceProjects: FindProjects(path, "src"),
            TestProjects: FindProjects(path, "test"),
            ExampleProjects: FindProjects(path, "example"));
    }

    private static bool IsAdditionalItem(string path)
    {
        return Path.GetExtension(path) switch
        {
            ".props" or ".targets" => true,
            ".md" or ".txt" => true,
            _ => false,
        };
    }

    private static string[] FindProjects(string path, string subdirectory)
    {
        var fullPath = Path.Combine(path, subdirectory);
        return Directory.Exists(fullPath) ? Directory.GetFiles(fullPath, "*.csproj", SearchOption.AllDirectories) : [];
    }

    private static XElement? GetFileFolder(string[] items, string folderName, string solutionPath)
    {
        if (items.Length is 0)
        {
            return null;
        }

        var projects = items.Select(path => new XElement("File", new XAttribute("Path", Path.GetRelativePath(solutionPath, path))));
        return new XElement("Folder", [new XAttribute("Name", $"/{folderName}/"), ..projects]);
    }

    private static XElement? GetProjectFolder(string[] items, string folderName, string solutionPath)
    {
        if (items.Length is 0)
        {
            return null;
        }

        var projects = items.Select(path => new XElement("Project", new XAttribute("Path", Path.GetRelativePath(solutionPath, path))));
        return new XElement("Folder", [new XAttribute("Name", $"/{folderName}/"), ..projects]);
    }

    private static void WriteXml(XDocument document, string targetPath)
    {
        using var fileStream = File.Create(targetPath);
        var writer = new XmlTextWriter(fileStream, Encoding.UTF8) { Formatting = Formatting.Indented };
        document.WriteTo(writer);
        writer.Flush();
    }

    private sealed record Project(
        string Name,
        string[] AdditionalItems,
        string[] SourceProjects,
        string[] TestProjects,
        string[] ExampleProjects);
}

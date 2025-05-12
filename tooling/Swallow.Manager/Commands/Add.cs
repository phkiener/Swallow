using System.Reflection;

namespace Swallow.Manager.Commands;

public static class Add
{
    private const string TemplateFilePrefix = "Swallow.Manager.Template.";

    public static int Run(string directory, string name)
    {
        var projectDirectory = Path.Combine(directory, "projects", $"Swallow.{name}");
        if (Directory.Exists(projectDirectory))
        {
            Console.WriteLine($"Project Swallow.{name} already exists");
            return 1;
        }

        Directory.CreateDirectory(projectDirectory);
        Directory.CreateDirectory(Path.Combine(projectDirectory, "doc"));
        Directory.CreateDirectory(Path.Combine(projectDirectory, "example"));
        Directory.CreateDirectory(Path.Combine(projectDirectory, "src"));
        Directory.CreateDirectory(Path.Combine(projectDirectory, "test"));

        var templateVariables = new Dictionary<string, string> { ["name"] = name, ["year"] = DateTime.Now.Year.ToString() };
        foreach (var templateFile in GetTemplateFiles())
        {
            try
            {
                CopyTemplateFile(projectDirectory, templateFile, templateVariables);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 1;
            }
        }

        Generate.Run(directory);

        return 0;
    }

    private static IEnumerable<string> GetTemplateFiles()
    {
        return Assembly.GetExecutingAssembly()
            .GetManifestResourceNames()
            .Where(static file => file.StartsWith(TemplateFilePrefix));
    }

    private static void CopyTemplateFile(string projectDirectory, string templateFile, IReadOnlyDictionary<string, string> templateVariables)
    {
        var targetName = templateFile[TemplateFilePrefix.Length..];

        using var templateStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(templateFile);
        if (templateStream is null)
        {
            throw new InvalidOperationException("Template file is missing from assembly");
        }

        using var fileReader = new StreamReader(templateStream);
        var templateContent = fileReader.ReadToEnd();

        var targetContent = templateVariables.Aggregate(
            templateContent,
            static (text, variable) => text.Replace($"#{{{variable.Key}}}", variable.Value, StringComparison.OrdinalIgnoreCase));

        var targetPath = Path.Combine(projectDirectory, targetName);
        File.WriteAllText(targetPath, targetContent);
    }
}

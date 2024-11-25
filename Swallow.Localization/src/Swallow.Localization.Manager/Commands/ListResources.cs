using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Swallow.Localization.Manager.Localizations;
using Swallow.Localization.Manager.Resources;

namespace Swallow.Localization.Manager.Commands;

public sealed class ListResources
{
    public static async Task RunAsync(string[] args)
    {
        var target = args.LastOrDefault() is not null and var path
            ? FindSolution(path, recurseUpwards: false)
            : FindSolution(Environment.CurrentDirectory, recurseUpwards: true);

        if (target is null)
        {
            throw new InvalidOperationException("No solution or project was found.");
        }

        var workspace = await OpenSolutionAsync(target);

        var resources = new HashSet<ResourceKey>();
        var localizations = new HashSet<LocalizedText>();

        foreach (var project in workspace.CurrentSolution.Projects)
        {
            await foreach (var resource in ResourceFinder.FindResourcesAsync(project))
            {
                resources.Add(resource);
            }

            await foreach (var localization in LocalizationFinder.FindLocalizationsAsync(project))
            {
                localizations.Add(localization);
            }
        }

        foreach (var localization in localizations)
        {
            if (localization.Scope is null)
            {
                Console.WriteLine($"[{localization.Project}/unknown scope]: {localization.Identifier} => no information");
            }
            else
            {
                var matchingResourceKey = new ResourceKey(localization.Project, localization.Scope, localization.Identifier);
                var isDeclared = resources.Contains(matchingResourceKey);

                Console.WriteLine($"[{localization.Project}/{localization.Scope}]: {localization.Identifier} => {(isDeclared ? "found" : "not found")}");
            }
        }
    }

    private static string? FindSolution(string fileOrPath, bool recurseUpwards)
    {
        if (File.Exists(fileOrPath))
        {
            return fileOrPath;
        }

        if (Directory.Exists(fileOrPath))
        {
            var solutionFiles = Directory.GetFiles(fileOrPath, "*.sln");
            if (solutionFiles is [var singleSolution])
            {
                return singleSolution;
            }

            var projectFiles = Directory.GetFiles(fileOrPath, "*.csproj");
            if (projectFiles is [var singleProject])
            {
                return singleProject;
            }
        }

        if (recurseUpwards)
        {
            var parentDirectory = Directory.GetParent(fileOrPath);
            if (parentDirectory is not null)
            {
                return FindSolution(parentDirectory.FullName, recurseUpwards);
            }
        }

        return null;
    }

    private static async Task<Workspace> OpenSolutionAsync(string solutionPath)
    {
        MSBuildLocator.RegisterDefaults();

        var workspace = MSBuildWorkspace.Create();
        await workspace.OpenSolutionAsync(solutionPath);

        return workspace;
    }

}

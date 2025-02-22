﻿using Swallow.TaskRunner.Abstractions;
using Swallow.TaskRunner.Serialization;

namespace Swallow.TaskRunner.Commands;

public sealed class CreateManifest : ICommand
{
    public async Task<int> RunAsync(ICommandContext console, string[] args)
    {
        var directory = Path.Combine(console.CurrentDirectory, ".config");
        Directory.CreateDirectory(directory);

        var filePath = Path.Combine(directory, "dotnet-tasks.json");
        if (File.Exists(filePath))
        {
            await console.Output.WriteLineAsync($"Task manifest already exists at {filePath}");
        }

        await ManifestWriter.WriteAsync(Manifest.Create(), filePath, console.CancellationToken);
        await console.Output.WriteLineAsync($"Created new task manifest in {filePath}");

        return 0;
    }
}

﻿using Swallow.TaskRunner.Abstractions;
using Swallow.TaskRunner.Serialization;

namespace Swallow.TaskRunner.Commands;

public sealed class InvokeTask : ICommand
{
    public async Task<int> RunAsync(ICommandContext console, string[] args)
    {
        var manifestFilePath = ManifestReader.FindManifestFile(console.CurrentDirectory);
        if (manifestFilePath is null)
        {
            throw new InvalidOperationException("No manifest file found.");
        }

        await using var fileStream = File.OpenRead(manifestFilePath);
        var manifest = await ManifestReader.ReadAsync(fileStream, console.CancellationToken);

        var taskName = string.Join(" ", args);
        var task = manifest.FindTask(taskName);
        if (task is null)
        {
            throw new InvalidOperationException($"No task '{taskName}' found.");
        }

        return await task.RunAsync(console);
    }
}

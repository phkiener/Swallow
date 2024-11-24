using Swallow.TaskRunner.Abstractions;

namespace Swallow.TaskRunner.Commands;

public sealed class DisplayHelp : ICommand
{
    public async Task<int> RunAsync(ICommandContext console, string[] args)
    {
        await console.Output.WriteLineAsync("dotnet task - Shortcuts for CLI commands");
        await console.Output.WriteLineAsync();
        await console.Output.WriteLineAsync("Usage:");
        await console.Output.WriteLineAsync("  dotnet task new-manifest    - Create a new manifest in the current location");
        await console.Output.WriteLineAsync("  dotnet task list            - List all tasks found in the current manifest");
        await console.Output.WriteLineAsync("  dotnet task <name>          - Invoke the task called <name> from the current manifest");
        await console.Output.WriteLineAsync("  dotnet task [-h|--help]     - Print this help text");
        await console.Output.WriteLineAsync("  dotnet task [-v|--version]  - Print version information");
        await console.Output.WriteLineAsync();
        await console.Output.WriteLineAsync("Task manifest:");
        await console.Output.WriteLineAsync("  The manifest is assumed to be found at .config/dotnet-tasks.json.");
        await console.Output.WriteLineAsync("  If no matching file is found, the parent directory and its parents in turn will be searched until such a file is found.");
        await console.Output.WriteLineAsync();

        return 0;
    }
}

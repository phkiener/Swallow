using System.Reflection;

namespace Swallow.TaskRunner.Commands;

public sealed class DisplayVersion : ICommand
{
    public async Task<int> Run(CommandContext console, string[] args)
    {
        var assemblyVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        await console.Output.WriteLineAsync(assemblyVersion?.InformationalVersion ?? "ultra rare preview version");

        return 0;
    }
}

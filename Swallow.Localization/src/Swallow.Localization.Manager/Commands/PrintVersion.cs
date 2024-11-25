using System.Reflection;

namespace Swallow.Localization.Manager.Commands;

public sealed class PrintVersion
{
    public static async Task RunAsync()
    {
        var assemblyVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        await Console.Out.WriteLineAsync(assemblyVersion?.InformationalVersion ?? "ultra rare preview version");
    }
}

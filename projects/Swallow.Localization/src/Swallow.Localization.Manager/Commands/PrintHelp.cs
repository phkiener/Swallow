namespace Swallow.Localization.Manager.Commands;

public sealed class PrintHelp
{
    public static async Task RunAsync()
    {
        await Console.Out.WriteLineAsync("dotnet localize - A tool to view, validate and manage localizations");
        await Console.Out.WriteLineAsync();
    }
}

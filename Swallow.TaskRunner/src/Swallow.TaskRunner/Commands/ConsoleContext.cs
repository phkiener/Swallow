using System.Diagnostics;

namespace Swallow.TaskRunner.Commands;

public sealed class ConsoleContext : ICommandContext
{
    public TextWriter Output => Console.Out;
    public TextWriter Error => Console.Error;
    public string CurrentDirectory => Environment.CurrentDirectory;

    public async Task<int> Execute(string command)
    {
        var shell = GetShell();
        var process = new Process { StartInfo = new ProcessStartInfo(shell, $"-c \"{command}\"") };
        process.Start();

        await process.WaitForExitAsync();
        return process.ExitCode;
    }

    private static string GetShell()
    {
        var environmentVariable = Environment.GetEnvironmentVariable("SHELL");
        if (environmentVariable is not null)
        {
            return environmentVariable;
        }

        return Environment.OSVersion.Platform switch
        {
            PlatformID.Win32S => "pwsh",
            PlatformID.Win32Windows => "pwsh",
            PlatformID.Win32NT => "pwsh",
            PlatformID.WinCE => "pwsh",
            PlatformID.Unix => "sh",
            PlatformID.MacOSX => "sh",
            PlatformID.Other => "sh",
            _ => throw new NotSupportedException($"Cannot automatically determine shell on {Environment.OSVersion.Platform}")
        };
    }
}

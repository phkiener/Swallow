using Swallow.TaskRunner.Commands;
using Xunit;

namespace Swallow.TaskRunner.Test.Commands;

public sealed class CreateManifestTest
{
    private readonly CreateManifest command = new();

    [Fact]
    public async Task CreatesNewManifest()
    {
        using var context = TestCommandContext.Create();
        await command.Run(context, []);

        var expectedFile = Path.Combine(context.CurrentDirectory, ".config", "dotnet-tasks.json");
        Assert.Contains(expectedFile, context.WrittenOutput);
        Assert.Contains("Created new task manifest", context.WrittenOutput);

        Assert.True(File.Exists(expectedFile), "Manifest file was not created");
    }

    [Fact]
    public async Task DoesNothing_WhenManifestExists_InCurrentDirectory()
    {
        using var context = TestCommandContext.Create();
        await command.Run(context, []);
        await command.Run(context, []);

        Assert.Contains("Task manifest already exists", context.WrittenOutput);
    }
}

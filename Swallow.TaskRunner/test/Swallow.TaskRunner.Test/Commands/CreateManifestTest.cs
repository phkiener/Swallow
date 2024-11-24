using Swallow.TaskRunner.Commands;
using Swallow.TaskRunner.Serialization;
using Xunit;

namespace Swallow.TaskRunner.Test.Commands;

public sealed class CreateManifestTest
{
    private readonly CreateManifest command = new();

    [Fact]
    public async Task CreatesNewManifest()
    {
        using var context = TestCommandContext.Create();
        await command.RunAsync(context, []);

        var expectedFile = Path.Combine(context.CurrentDirectory, ".config", "dotnet-tasks.json");
        Assert.Contains(expectedFile, context.WrittenOutput);
        Assert.Contains("Created new task manifest", context.WrittenOutput);

        await using var fileStream = File.OpenRead(expectedFile);
        var manifest = ManifestReader.ReadAsync(fileStream, context.CancellationToken);

        Assert.NotNull(manifest);
    }

    [Fact]
    public async Task RunningTwice_WorksWithoutExceptions()
    {
        using var context = TestCommandContext.Create();
        await command.RunAsync(context, []);
        await command.RunAsync(context, []);

        Assert.Contains("Task manifest already exists", context.WrittenOutput);
    }
}

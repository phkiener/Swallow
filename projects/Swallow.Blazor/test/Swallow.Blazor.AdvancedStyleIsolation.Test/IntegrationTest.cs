using System.Diagnostics;

namespace Swallow.Blazor.AdvancedStyleIsolation;

[TestFixture]
public sealed class IntegrationTest
{
    private string exampleProjectPath = "";
    private string outputPath = "";

    [SetUp]
    public void SetUp()
    {
        var assemblyPath = typeof(IntegrationTest).Assembly.Location;
        var projectPath = Path.GetFullPath(Path.Combine(assemblyPath, "../../../../"));

        exampleProjectPath = Path.Combine(projectPath, "../Swallow.Blazor.AdvancedStyleIsolation.ExampleProject");
        outputPath = Path.Combine(projectPath, "out");
    }

    [Test]
    public async Task ProjectIsBuiltCorrectly()
    {
        await RenderFilesAsync();

        // Baseline
        await AssertRenderedFileAsync(filename: "Base.rendered", expectedAttribute: "b-vidivxz9sz", expectedScope: "b-vidivxz9sz");
        await AssertRenderedFileAsync(filename: "DerivedDefault.rendered", expectedAttribute: "b-58vjkcu8nm", expectedScope: "b-58vjkcu8nm");
        await AssertRenderedFileAsync(filename: "DerivedDefaultNoStyle.rendered", expectedAttribute: null, expectedScope: null);

        // With inherit
        await AssertRenderedFileAsync(filename: "DerivedInherit.rendered", expectedAttribute: "b-vidivxz9sz", expectedScope: "b-vidivxz9sz");
        await AssertRenderedFileAsync(filename: "DerivedInheritNoStyle.rendered", expectedAttribute: "b-vidivxz9sz", expectedScope: null);

        // With append
        await AssertRenderedFileAsync(filename: "DerivedAppend.rendered", expectedAttribute: "b-sbp7vt9gm8 b-vidivxz9sz", expectedScope: "b-sbp7vt9gm8");
        await AssertRenderedFileAsync(filename: "DerivedAppendNoStyle.rendered", expectedAttribute: "b-vidivxz9sz", expectedScope: null);
    }

    private async Task RenderFilesAsync()
    {
        var dotnetPath = Environment.ProcessPath;
        if (dotnetPath is null)
        {
            Assert.Inconclusive("Could not locate dotnet executable.");
        }

        var processInfo = new ProcessStartInfo(dotnetPath)
        {
            Arguments = $"run --project {Path.GetFullPath(exampleProjectPath)} -- {Path.GetFullPath(outputPath)}",
            UseShellExecute = false,
            RedirectStandardInput = false,
            RedirectStandardOutput = false,
            RedirectStandardError = false,
            WindowStyle = ProcessWindowStyle.Hidden
        };

        var process = Process.Start(processInfo);
        if (process is null)
        {
            Assert.Inconclusive("Could not invoke dotnet.");
        }

        using var cancellationToken = new CancellationTokenSource(TimeSpan.FromMinutes(1));
        await process.WaitForExitAsync(cancellationToken.Token);

        if (process.ExitCode != 0)
        {
            Assert.Fail("dotnet returned non-zero exit code.");
        }
    }

    private async Task AssertRenderedFileAsync(string filename, string expectedAttribute, string expectedScope)
    {
        var content = await File.ReadAllTextAsync(Path.Combine(outputPath, filename));

        var expectedMarkup = expectedAttribute is null ? "<div></div>" : $"<div {expectedAttribute}></div>";
        Assert.That(content, Does.Contain(expectedMarkup));
        Assert.That(content, expectedScope is null ? Does.Not.Contain("color: green") : Does.Contain($"div[{expectedScope}]"));
    }
}

using System.Diagnostics;

namespace Swallow.Build.StyleIsolation;

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

        exampleProjectPath = Path.Combine(projectPath, "../Swallow.Build.StyleIsolation.ExampleProject");
        outputPath = Path.Combine(projectPath, "out");
    }

    [Test]
    public async Task ProjectIsBuiltCorrectly()
    {
        await RenderFilesAsync();

        await AssertRenderedFileAsync(filename: "Base.rendered", expectedAttribute: "b-qsyb1vuczk", expectedScope: "b-qsyb1vuczk");
        await AssertRenderedFileAsync(filename: "DerivedDefault.rendered", expectedAttribute: "b-zz8pluwsp4", expectedScope: "b-zz8pluwsp4");
        await AssertRenderedFileAsync(filename: "DerivedDefaultNoStyle.rendered", expectedAttribute: null, expectedScope: null);
        await AssertRenderedFileAsync(filename: "DerivedInherit.rendered", expectedAttribute: "b-qsyb1vuczk", expectedScope: "b-qsyb1vuczk");
        await AssertRenderedFileAsync(filename: "DerivedInheritNoStyle.rendered", expectedAttribute: "b-qsyb1vuczk", expectedScope: null);
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

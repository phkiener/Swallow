namespace Swallow.TaskRunner.Test;

public sealed class TestCommandContext : ICommandContext, IDisposable
{
    private static readonly string TestContextDirectory = Path.Combine(Path.GetTempPath(), "Swallow.TaskRunner", nameof(TestCommandContext));
    private readonly string initialDirectory;

    private TestCommandContext(string directory)
    {
        initialDirectory = directory;
        CurrentDirectory = initialDirectory;
    }

    public TextWriter Output { get; } = new StringWriter();
    public TextWriter Error { get; } = new StringWriter();
    public string CurrentDirectory { get; }

    public string WrittenOutput => Output.ToString() ?? "";
    public string WrittenError=> Error.ToString() ?? "";

    public void Dispose()
    {
        Directory.Delete(initialDirectory, recursive: true);

        Output.Dispose();
        Error.Dispose();
    }

    public static TestCommandContext Create()
    {
        var contextPath = Path.Combine(TestContextDirectory, Guid.NewGuid().ToString("D"));
        Directory.CreateDirectory(contextPath);

        return new TestCommandContext(contextPath);
    }
}

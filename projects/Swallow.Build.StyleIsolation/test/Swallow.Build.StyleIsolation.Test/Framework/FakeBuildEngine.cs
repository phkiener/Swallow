using System.Collections;
using Microsoft.Build.Framework;

namespace Swallow.Build.StyleIsolation;

public sealed class FakeBuildEngine : IBuildEngine
{
    private readonly List<string> logs = [];

    public IEnumerable<string> Logs => logs;
    public bool ContinueOnError => false;
    public int LineNumberOfTaskNode => 1;
    public int ColumnNumberOfTaskNode => 1;
    public string ProjectFileOfTaskNode => "FakeProject.csproj";

    public void LogErrorEvent(BuildErrorEventArgs e)
    {
        logs.Add($"ERR: {e.Message}");
    }

    public void LogWarningEvent(BuildWarningEventArgs e)
    {
        logs.Add($"WARN: {e.Message}");
    }

    public void LogMessageEvent(BuildMessageEventArgs e)
    {
        logs.Add($"MSG: {e.Message}");
    }

    public void LogCustomEvent(CustomBuildEventArgs e)
    {
        logs.Add($"CUST: {e.Message}");
    }

    public bool BuildProjectFile(string projectFileName, string[] targetNames, IDictionary globalProperties, IDictionary targetOutputs)
    {
        return false;
    }
}

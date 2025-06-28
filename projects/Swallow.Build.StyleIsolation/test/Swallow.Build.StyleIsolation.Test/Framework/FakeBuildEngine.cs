using System.Collections;
using Microsoft.Build.Framework;

namespace Swallow.Build.StyleIsolation;

public sealed class FakeBuildEngine : IBuildEngine
{
    public bool ContinueOnError => false;
    public int LineNumberOfTaskNode => 1;
    public int ColumnNumberOfTaskNode => 1;
    public string ProjectFileOfTaskNode => "FakeProject.csproj";

    public void LogErrorEvent(BuildErrorEventArgs e) { }

    public void LogWarningEvent(BuildWarningEventArgs e) { }

    public void LogMessageEvent(BuildMessageEventArgs e) { }

    public void LogCustomEvent(CustomBuildEventArgs e) { }

    public bool BuildProjectFile(string projectFileName, string[] targetNames, IDictionary globalProperties, IDictionary targetOutputs)
    {
        return false;
    }
}

using Microsoft.Extensions.FileProviders;

namespace Swallow.SiteGen;

public sealed class BuildOptions : IWebHostEnvironment
{
    public BuildOptions(string targetPath)
    {
        ContentRootFileProvider = new PhysicalFileProvider(ContentRootPath);
        WebRootFileProvider = new PhysicalFileProvider(WebRootPath);

        TargetPath = targetPath;
    }

    public string TargetPath { get; }

    public string ApplicationName { get; set; } = "Swallow SiteGen";
    public string EnvironmentName { get; set; } = "Development";

    public string ContentRootPath { get; set; } = Environment.CurrentDirectory;
    public string WebRootPath { get; set; } = Path.Combine(Environment.CurrentDirectory,  "wwwroot");

    public IFileProvider ContentRootFileProvider { get; set; }
    public IFileProvider WebRootFileProvider { get; set; }
}

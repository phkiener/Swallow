using System.Reflection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Swallow.SiteGen;

public sealed class BuildOptions : IWebHostEnvironment
{
    public BuildOptions(string targetPath)
    {
        var assemblyLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;

        ContentRootPath = assemblyLocation;
        ContentRootFileProvider = new PhysicalFileProvider(ContentRootPath);

        WebRootPath = Path.Combine(assemblyLocation, "wwwroot");
        WebRootFileProvider = Directory.Exists(WebRootPath) ? new PhysicalFileProvider(WebRootPath) : new EmptyFileProvider();

        TargetPath = targetPath;
    }

    public string TargetPath { get; }

    public string ApplicationName { get; set; } = "Swallow.SiteGen";
    public string EnvironmentName { get; set; } = "Development";

    public string ContentRootPath { get; set; }
    public string WebRootPath { get; set; }

    public IFileProvider ContentRootFileProvider { get; set; }
    public IFileProvider WebRootFileProvider { get; set; }

    private sealed class EmptyFileProvider : IFileProvider
    {
        public IDirectoryContents GetDirectoryContents(string subpath) => new NotFoundDirectoryContents();

        public IFileInfo GetFileInfo(string subpath) => new NotFoundFileInfo(subpath);

        public IChangeToken Watch(string filter) => NullChangeToken.Singleton;
    }
}

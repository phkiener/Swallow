using Microsoft.AspNetCore.Components;
using NuGet.Common;
using NuGet.Protocol.Core.Types;

namespace Swallow.SiteGen.Pages.Sections;

public sealed partial class PackagesSection(SourceCacheContext cache, SourceRepository repository) : ComponentBase
{
    [Parameter]
    [EditorRequired]
    public required IEnumerable<string> Packages { get; set; }

    private readonly List<PackageDetails> packageDetails = [];

    protected override async Task OnParametersSetAsync()
    {
        var metadataResource = await repository.GetResourceAsync<PackageMetadataResource>();
        if (metadataResource is null)
        {
            throw new InvalidOperationException($"{nameof(PackageMetadataResource)} resource is null");
        }

        foreach (var package in Packages)
        {
            var metadata = await metadataResource.GetMetadataAsync(
                packageId: package,
                includePrerelease: false,
                includeUnlisted: false,
                sourceCacheContext: cache,
                log: NullLogger.Instance,
                token: CancellationToken.None);

            var lastVersion = metadata.LastOrDefault();
            if (lastVersion is null)
            {
                continue;
            }

            var details = new PackageDetails(
                Name: lastVersion.Title,
                Description: lastVersion.Description,
                NuGetUrl: lastVersion.PackageDetailsUrl.ToString(),
                GitHubUrl: $"https://github.com/phkiener/{lastVersion.Title}");

            packageDetails.Add(details);
        }
    }

    private sealed record PackageDetails(string Name, string Description, string NuGetUrl, string GitHubUrl);
}

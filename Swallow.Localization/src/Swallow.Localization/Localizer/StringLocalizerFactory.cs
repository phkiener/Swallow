using System.Reflection;
using System.Resources;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swallow.Localization.Language;

namespace Swallow.Localization.Localizer;

/// <summary>
/// Factory for a string localizer that translates resources based on the given <see cref="ILanguageProvider"/>.
/// </summary>
/// <inheritdoc cref="ResourceManagerStringLocalizer"/>
public sealed class StringLocalizerFactory(
    IOptions<LocalizationOptions> localizationOptions,
    ILoggerFactory loggerFactory,
    ILanguageProvider languageProvider)
    : ResourceManagerStringLocalizerFactory(localizationOptions, loggerFactory)
{
    private readonly IResourceNamesCache resourceNamesCache = new ResourceNamesCache();
    private readonly ILoggerFactory loggerFactory = loggerFactory;

    /// <inheritdoc />
    protected override ResourceManagerStringLocalizer CreateResourceManagerStringLocalizer(Assembly assembly, string baseName)
    {
        return new StringLocalizer(
            resourceManager: new ResourceManager(baseName, assembly),
            resourceAssembly: assembly,
            baseName: baseName,
            resourceNamesCache: resourceNamesCache,
            logger: loggerFactory.CreateLogger<StringLocalizer>(),
            languageProvider: languageProvider);
    }
}

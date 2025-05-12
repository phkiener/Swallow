using System.Reflection;
using System.Resources;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Swallow.Localization.Language;

namespace Swallow.Localization.Localizer;

/// <summary>
/// A string localizer that translates resources based on the given <see cref="ILanguageProvider"/>.
/// </summary>
/// <inheritdoc cref="ResourceManagerStringLocalizer"/>
public sealed class StringLocalizer(
    ResourceManager resourceManager,
    Assembly resourceAssembly,
    string baseName,
    IResourceNamesCache resourceNamesCache,
    ILogger logger,
    ILanguageProvider languageProvider)
    : ResourceManagerStringLocalizer(
        resourceManager: resourceManager,
        resourceAssembly: resourceAssembly,
        baseName: baseName,
        resourceNamesCache: resourceNamesCache,
        logger: logger)
{
    private readonly string resourceBaseName = baseName;

    /// <inheritdoc />
    public override LocalizedString this[string name] => Resolve(name);

    /// <inheritdoc />
    public override LocalizedString this[string name, params object[] arguments] => Resolve(name, arguments);

    /// <inheritdoc />
    public override IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) => GetAllStrings(includeParentCultures, languageProvider.LanguageCulture);

    private LocalizedString Resolve(string name, object[]? args = null)
    {
        var value = GetStringSafely(name, languageProvider.LanguageCulture);
        if (args is not null)
        {
            value = string.Format(languageProvider.FormatCulture, value ?? name, args);
        }

        return new LocalizedString(name, value ?? name, resourceNotFound: value == null, searchedLocation: resourceBaseName);
    }
}

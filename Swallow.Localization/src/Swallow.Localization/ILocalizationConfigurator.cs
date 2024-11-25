using System.Globalization;
using Microsoft.Extensions.Localization;
using Swallow.Localization.Localizer;

namespace Swallow.Localization;

/// <summary>
/// Configuration options for the <see cref="StringLocalizer"/>
/// </summary>
public interface ILocalizationConfigurator
{
    /// <summary>
    /// Only use the given cultures to translate resources.
    /// </summary>
    /// <param name="languageCulture">The culture to use for translation.</param>
    /// <param name="formatCulture">The culture to use for formatting; defaults to <paramref name="languageCulture"/>.</param>
    public ILocalizationConfigurator UseFixedLanguage(CultureInfo languageCulture, CultureInfo? formatCulture = null);

    /// <summary>
    /// Use <see cref="CultureInfo.CurrentUICulture"/> and <see cref="CultureInfo.CurrentCulture"/> to translate resources.
    /// </summary>
    /// <remarks>
    /// Using this is equivalent to just using the <see cref="ResourceManagerStringLocalizerFactory"/> on its own.
    /// </remarks>
    public ILocalizationConfigurator UseCultureInfo();
}

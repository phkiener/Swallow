using System.Globalization;

namespace Swallow.Localization.Language;

/// <summary>
/// A <see cref="ILanguageProvider"/> that works as an adapter for <see cref="CultureInfo.CurrentUICulture"/> and
/// <see cref="CultureInfo.CurrentCulture"/>.
/// </summary>
public sealed class CultureInfoLanguageProvider : ILanguageProvider
{
    /// <inheritdoc />
    public CultureInfo LanguageCulture => CultureInfo.CurrentUICulture;

    /// <inheritdoc />
    public CultureInfo FormatCulture => CultureInfo.CurrentCulture;
}

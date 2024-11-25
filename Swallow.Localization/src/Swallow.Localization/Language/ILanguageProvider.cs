using System.Globalization;

namespace Swallow.Localization.Language;

/// <summary>
/// A language provider that declares which language to use for translation and formatting.
/// </summary>
public interface ILanguageProvider
{
    /// <summary>
    /// The culture to use for translation.
    /// </summary>
    public CultureInfo LanguageCulture { get; }

    /// <summary>
    /// The culture to use for formatting.
    /// </summary>
    public CultureInfo FormatCulture { get; }
}

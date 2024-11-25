using System.Globalization;

namespace Swallow.Localization.Language;

/// <summary>
/// A <see cref="ILanguageProvider"/> that will provide a fixed culture over its lifetime.
/// </summary>
public sealed class FixedLanguageProvider(CultureInfo languageCulture, CultureInfo? formatCulture = null) : ILanguageProvider
{
    /// <inheritdoc />
    public CultureInfo LanguageCulture { get; } = languageCulture;

    /// <inheritdoc />
    public CultureInfo FormatCulture { get; } = formatCulture ?? languageCulture;
}

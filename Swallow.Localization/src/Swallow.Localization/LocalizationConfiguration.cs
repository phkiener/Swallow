using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Swallow.Localization.Language;
using Swallow.Localization.Localizer;

namespace Swallow.Localization;

/// <summary>
/// Methods to register the deterministic localization.
/// </summary>
public static class ServiceProviderConfiguration
{
    /// <summary>
    /// Register a special <see cref="IStringLocalizer"/> that will translate resources based on a <see cref="ILanguageProvider"/>.
    /// </summary>
    public static ILocalizationConfigurator AddSwallowLocalization(this IServiceCollection services)
    {
        services.AddLocalization();
        services.Replace(ServiceDescriptor.Singleton(typeof(IStringLocalizerFactory), typeof(StringLocalizerFactory)));

        return new LocalizationConfigurator(services);
    }

    private sealed class LocalizationConfigurator(IServiceCollection services) : ILocalizationConfigurator
    {
        ILocalizationConfigurator ILocalizationConfigurator.UseFixedLanguage(CultureInfo languageCulture, CultureInfo? formatCulture)
        {
            services.AddSingleton<ILanguageProvider>(new FixedLanguageProvider(languageCulture, formatCulture));

            return this;
        }

        ILocalizationConfigurator ILocalizationConfigurator.UseCultureInfo()
        {
            services.AddSingleton<ILanguageProvider>(new CultureInfoLanguageProvider());

            return this;
        }
    }
}

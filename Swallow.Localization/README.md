# Swallow.Localization

A modified version of a `IStringLocalizer<T>` that does not rely on `CultureInfo.CurrentUiCulture` but instead on a `ILanguageProvider`.
This language provider can be any implementation - an adapter to the static `CultureInfo` properties, a version that returns fixed languages set
at construction time or... a random culture on every access, if you'd like.

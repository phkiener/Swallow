using Microsoft.Extensions.Localization;

namespace Swallow.Localization.Example;

public sealed class GlobalResourcesExample(IStringLocalizer<Translations> localizer)
{
    public void Translate()
    {
        _ = localizer["Hello, World!"];
        _ = localizer["My Name is: {0}", Environment.UserName];
    }
}

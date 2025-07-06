using Microsoft.AspNetCore.Components.HtmlRendering.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Swallow.Blazor.Reactive.Rendering;

internal sealed class StatefulReactiveComponentRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    : StaticHtmlRenderer(serviceProvider, loggerFactory);

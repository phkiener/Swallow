using Microsoft.AspNetCore.Components.HtmlRendering.Infrastructure;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.Logging;

namespace Swallow.Blazor.Reactive.Rendering;

internal sealed class StatefulReactiveComponentRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    : StaticHtmlRenderer(serviceProvider, loggerFactory)
{
    private readonly Dictionary<EventHandlerKey, EventHandler> eventHandlerByIdentifier = new();

    protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
    {
        for (var i = 0; i < renderBatch.ReferenceFrames.Count; i++)
        {
            var frame = renderBatch.ReferenceFrames.Array[i];
            if (IsEventHandler(ref frame))
            {
                var elementFrames = GetElementFrames(renderBatch.ReferenceFrames, i);
                var componentId = GetComponentId(renderBatch.ReferenceFrames, i);

                foreach (var elementFrame in elementFrames)
                {
                    if (elementFrame is { FrameType: RenderTreeFrameType.Attribute, AttributeName: "rx-id", AttributeValue: string id })
                    {
                        var key = new EventHandlerKey(id, frame.AttributeName);
                        var value = new EventHandler(frame.AttributeEventHandlerId, componentId);

                        eventHandlerByIdentifier[key] = value;
                    }
                }
            }
        }

        return Task.CompletedTask;
    }

    public Task DispatchEventAsync(string identifier, string eventName)
    {
        var key = new EventHandlerKey(identifier, "on" + eventName);
        if (eventHandlerByIdentifier.TryGetValue(key, out var value))
        {
            var eventArgsType = GetEventArgsType(value.HandlerId);
            var eventArgs = eventArgsType == typeof(EventArgs) ? EventArgs.Empty : (EventArgs)Activator.CreateInstance(eventArgsType)!;
            var fieldInfo = new EventFieldInfo { ComponentId = value.ComponentId };

            return DispatchEventAsync(value.HandlerId, fieldInfo, eventArgs, true);
        }

        return Task.CompletedTask;
    }

    private static bool IsEventHandler(ref RenderTreeFrame frame)
    {
        return frame is { FrameType: RenderTreeFrameType.Attribute, AttributeEventHandlerId: not 0 };
    }

    private static ReadOnlySpan<RenderTreeFrame> GetElementFrames(ArrayRange<RenderTreeFrame> frames, int frameIndex)
    {
        var elementStart = frameIndex;
        while (frames.Array[elementStart].FrameType is not (RenderTreeFrameType.Element or RenderTreeFrameType.Component))
        {
            elementStart -= 1;
        }

        var elementEnd = frameIndex;
        while (frames.Array[elementEnd].FrameType is RenderTreeFrameType.Attribute
               or RenderTreeFrameType.ElementReferenceCapture
               or RenderTreeFrameType.ComponentReferenceCapture
               or RenderTreeFrameType.ComponentRenderMode)
        {
            elementEnd += 1;
        }

        return frames.Array.AsSpan(elementStart, elementEnd - elementStart);
    }

    private static int GetComponentId(ArrayRange<RenderTreeFrame> frames, int frameIndex)
    {
        for (var i = frameIndex; i > 0; i--)
        {
            var frame = frames.Array[i];
            if (frame.FrameType is RenderTreeFrameType.Component)
            {
                return frame.ComponentId;
            }
        }

        return 0;
    }

    private readonly record struct EventHandlerKey(string Identifier, string EventType);
    private readonly record struct EventHandler(ulong HandlerId, int ComponentId);
}

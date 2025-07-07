using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.HtmlRendering.Infrastructure;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.Logging;
using Swallow.Blazor.Reactive.Abstractions.Rendering;

namespace Swallow.Blazor.Reactive.Rendering;

internal sealed class StatefulReactiveComponentRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    : StaticHtmlRenderer(serviceProvider, loggerFactory)
{
    private static readonly IComponentRenderMode renderModeInstance = new ReactiveRenderMode();
    private static readonly JsonSerializerOptions EventSerializerOptions = new()
    {
        MaxDepth = 32,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    protected override IComponentRenderMode GetComponentRenderMode(IComponent component)
    {
        return renderModeInstance;
    }

    public Task DispatchEventAsync(string identifier, string eventName, string? eventBody)
    {
        var eventHandler = FindEventHandler(identifier, eventName);
        if (eventHandler.HasValue)
        {
            var eventArgsType = GetEventArgsType(eventHandler.Value.HandlerId);
            var eventArgs = eventBody is null
                ? (EventArgs)Activator.CreateInstance(eventArgsType)!
                : (EventArgs)JsonSerializer.Deserialize(eventBody, eventArgsType, EventSerializerOptions)!;

            var fieldInfo = new EventFieldInfo { ComponentId = eventHandler.Value.ComponentId };
            if (eventArgs is ChangeEventArgs { Value: JsonElement changedValue } changeEvent)
            {
                var actualValue = changedValue.Deserialize<string>(EventSerializerOptions);

                changeEvent.Value = actualValue;
                fieldInfo.FieldValue = actualValue ?? "";
            }

            return DispatchEventAsync(eventHandler.Value.HandlerId, fieldInfo, eventArgs, true);
        }

        return Task.CompletedTask;
    }

    private EventHandler? FindEventHandler(string identifier, string eventName)
    {
        const int rootComponentId = 1;
        var componentsQueue = new Queue<int>([rootComponentId]);

        while (componentsQueue.TryDequeue(out var componentId))
        {
            var frames = GetCurrentRenderTreeFrames(componentId);
            for (var i = 0; i < frames.Count; ++i)
            {
                var frame = frames.Array[i];
                if (frame.FrameType is RenderTreeFrameType.Component)
                {
                    componentsQueue.Enqueue(frame.ComponentId);
                    continue;
                }

                if (frame is { FrameType: RenderTreeFrameType.Attribute, AttributeEventHandlerId: not 0} && $"on{eventName}".Equals(frame.AttributeName))
                {
                    var elementFrames = GetElementFrames(frames, i);
                    foreach (var elementFrame in elementFrames)
                    {
                        if (elementFrame is { FrameType: RenderTreeFrameType.Attribute, AttributeName: "rx-id" } && identifier.Equals(elementFrame.AttributeValue))
                        {
                            return new EventHandler(frame.AttributeEventHandlerId, componentId);
                        }
                    }
                }
            }
        }

        return null;
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
               or RenderTreeFrameType.ComponentRenderMode
               or RenderTreeFrameType.NamedEvent)
        {
            elementEnd += 1;
        }

        return frames.Array.AsSpan(elementStart, elementEnd - elementStart);
    }

    private readonly record struct EventHandler(ulong HandlerId, int ComponentId);
}

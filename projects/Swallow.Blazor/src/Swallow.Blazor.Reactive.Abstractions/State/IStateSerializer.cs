namespace Swallow.Blazor.Reactive.Abstractions.State;

/// <summary>
/// A serializer used to transform component state into a text representation fit for the DOM.
/// </summary>
public interface IStateSerializer
{
    /// <summary>
    /// Serialize the given value to a HTML-safe representation.
    /// </summary>
    /// <param name="data">The data to serialize.</param>
    /// <returns>A string representation or null of the data should not be included.</returns>
    string? Serialize(object? data);

    /// <summary>
    /// Deserialize the given text as <paramref name="targetType"/>.
    /// </summary>
    /// <param name="targetType">The type to deserialize the data as.</param>
    /// <param name="text">The text to deserialize.</param>
    /// <returns>The deserialized object or null of no data exists.</returns>
    /// <seealso cref="Deserialize{T}"/>
    object? Deserialize(Type targetType, string? text);

    /// <summary>
    ///
    /// Deserialize the given text as <typeparamref name="T"/>.
    /// </summary>
    /// <param name="text">The text to deserialize.</param>
    /// <typeparam name="T">The type to deserialize the data as.</typeparam>
    /// <returns>The deserialized object or null of no data exists.</returns>
    /// <seealso cref="Deserialize"/>
    T? Deserialize<T>(string? text) => (T?)Deserialize(typeof(T), text);
}

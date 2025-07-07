using System.Linq.Expressions;
using Swallow.Blazor.Reactive.Abstractions.Rendering;

namespace Swallow.Blazor.Reactive.Abstractions.State;

/// <summary>
/// Handles the persisting and restoring of any state a component may hold.
/// </summary>
public interface IReactiveStateHandler
{
    /// <summary>
    /// Register the property pointed at by <paramref name="propertyExpression"/> to be included in the state.
    /// </summary>
    /// <param name="island">The reactive island to which the state belongs.</param>
    /// <param name="name">Name to store the state as.</param>
    /// <param name="propertyExpression">Expression to the property which contains the state to store.</param>
    /// <typeparam name="T">Type of state to manage.</typeparam>
    /// <returns><c>true</c> if state has been restored, <c>false</c> otherwise.</returns>
    /// <remarks>
    /// Directly when calling this method, the state is retrieved - if available. Then, at the
    /// end of rendering, the current value is persisted so that the next request may retrieve that
    /// value.
    /// </remarks>
    bool Register<T>(IReactiveIsland island, string name, Expression<Func<T>> propertyExpression);

    /// <summary>
    /// Remove any state registered by <see cref="Register{T}"/> from the handler.
    /// </summary>
    /// <param name="island">The reactive island to which the state belongs.</param>
    /// <param name="name">Name to store the state as.</param>
    /// <remarks>
    /// Should be called when a component is <see cref="IDisposable.Dispose"/>d.
    /// </remarks>
    void Remove(IReactiveIsland island, string name);
}

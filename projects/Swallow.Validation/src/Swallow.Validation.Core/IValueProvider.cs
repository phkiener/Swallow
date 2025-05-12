namespace Swallow.Validation;

/// <summary>
///     A value provider is everything that can provide a value.
/// </summary>
/// <remarks>
///     How that value is retrieved is up to the implementation - it may very well do
///     expensive calls to a remote server or just return a stored value.
/// </remarks>
/// <typeparam name="T">Type of the provided value.</typeparam>
public interface IValueProvider<out T>
{
    /// <summary>
    ///     Returns the value that is provided.
    /// </summary>
    T Value { get; }
}

/// <summary>
///     A named value provider is a <see cref="IValueProvider{T}" /> that also provides a name for the value.
/// </summary>
/// <typeparam name="T">Type of the provided value.</typeparam>
public interface INamedValueProvider<out T> : IValueProvider<T>
{
    /// <summary>
    ///     Returns the name for the provided value.
    /// </summary>
    string Name { get; }
}

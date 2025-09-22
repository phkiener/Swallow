using Swallow.Validation.Next.Internal;

namespace Swallow.Validation.Next.Asserters.Types;

/// <summary>
/// An error signaling that the asserted value has the wrong type.
/// </summary>
public sealed class WrongType(Type expectedType) : ValidationError
{
    /// <inheritdoc />
    public override string Message => $"have type {expectedType.FriendlyName()}";
}

/// <summary>
/// An asserter to check whether an object is of a certain type, produces <see cref="WrongType"/>
/// as validation error.
/// </summary>
/// <param name="expectedType">The type which the asserted value should have.</param>
/// <param name="allowDerivedTypes">
/// If <c>true</c>, a derived type of <paramref name="expectedType"/> is also accepted.
/// </param>
public sealed class IsInstanceOfAsserter(Type expectedType, bool allowDerivedTypes = false) : IAsserter<object>
{
    /// <inheritdoc />
    public bool IsValid(object value)
    {
        var type = value.GetType();

        return type == expectedType || allowDerivedTypes && type.IsAssignableTo(expectedType);
    }

    /// <inheritdoc />
    public ValidationError Error { get; } = new WrongType(expectedType);

    /// <summary>
    /// Return a new instance of <see cref="IsInstanceOfAsserter"/> that expects the asserted object
    /// to have type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type which the asserted value should have.</typeparam>
    /// <returns>A constructed asserter.</returns>
    public static IsInstanceOfAsserter Equals<T>()
    {
        return new IsInstanceOfAsserter(typeof(T), allowDerivedTypes: false);
    }

    /// <summary>
    /// Return a new instance of <see cref="IsInstanceOfAsserter"/> that expects the asserted object
    /// to have type <typeparamref name="T"/> or be derived from it.
    /// </summary>
    /// <typeparam name="T">The type which the asserted value should have or derive from.</typeparam>
    /// <returns>A constructed asserter.</returns>
    public static IsInstanceOfAsserter AssignableTo<T>()
    {
        return new IsInstanceOfAsserter(typeof(T), allowDerivedTypes: true);
    }
}

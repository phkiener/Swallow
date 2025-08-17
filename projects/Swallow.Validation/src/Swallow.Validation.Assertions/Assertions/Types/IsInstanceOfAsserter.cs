#nullable enable
using System.Diagnostics.CodeAnalysis;
using Swallow.Validation.Errors;
using Swallow.Validation.Internal;

namespace Swallow.Validation.Assertions.Text;

/// <summary>
/// An error signaling that the asserted value has the wrong type.
/// </summary>
public sealed class WrongType(Type expectedType) : ValidationError
{
    /// <inheritdoc />
    public override string Message => $"{PropertyName} is not of type {expectedType.FriendlyName()}";
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
    public bool Check(INamedValueProvider<object> valueProvider, [NotNullWhen(false)] out ValidationError? error)
    {
        if (valueProvider.Value.GetType() == expectedType)
        {
            error = null;
            return true;
        }

        if (allowDerivedTypes && valueProvider.Value.GetType().IsAssignableTo(expectedType))
        {
            error = null;
            return true;
        }

        error = new WrongType(expectedType);
        return false;
    }

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

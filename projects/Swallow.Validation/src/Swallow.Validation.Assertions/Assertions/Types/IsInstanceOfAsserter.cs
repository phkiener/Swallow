#nullable enable
using System.Diagnostics.CodeAnalysis;
using Swallow.Validation.Errors;
using Swallow.Validation.Internal;

namespace Swallow.Validation.Assertions.Text;

/// <summary>
/// An error signalling that the asserted value has the wrong type.
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
}

/// <summary>
/// A <see cref="IsInstanceOfAsserter"/> that accepts the expected type as type parameter.
/// </summary>
/// <param name="allowDerivedTypes">
/// If <c>true</c>, a derived type of <typeparamref name="T"/> is also accepted.
/// </param>
/// <typeparam name="T">The type which the asserted value should have.</typeparam>
public sealed class IsInstanceOfAsserter<T>(bool allowDerivedTypes = false) : IAsserter<object>
{
    // We wrap an instance of the non-generic asserter instead of inheriting so that both asserters may remain sealed.
    private readonly IsInstanceOfAsserter innerAsserter = new(typeof(T), allowDerivedTypes);

    /// <inheritdoc />
    public bool Check(INamedValueProvider<object> valueProvider, [NotNullWhen(false)] out ValidationError? error)
    {
        return innerAsserter.Check(valueProvider, out error);
    }
}

namespace Swallow.Validation.Errors;

using System;

/// <summary>
///     An error to signal that an object has an invalid type.
/// </summary>
public sealed class TypeValidationError : ValidationError
{
    /// <summary>
    ///     How the types should match.
    /// </summary>
    public enum MatchingMode
    {
        /// <summary>
        ///     The expected type should be equal to the actual type.
        /// </summary>
        SameType,

        /// <summary>
        ///     The actual type should be assignable to the expected type.
        /// </summary>
        AssignableTo
    }

    /// <summary>
    ///     Initializes an instance of the <see cref="TypeValidationError" /> class
    /// </summary>
    /// <param name="expectedType">The expected type for validation.</param>
    /// <param name="actualType">The actual type encountered.</param>
    /// <param name="mode">How the types are supposed to match.</param>
    public TypeValidationError(Type expectedType, Type actualType, MatchingMode mode)
    {
        ExpectedType = expectedType;
        ActualType = actualType;
        Mode = mode;
    }

    /// <summary>
    ///     The type that was expected.
    /// </summary>
    public Type ExpectedType { get; }

    /// <summary>
    ///     The actual type of the value.
    /// </summary>
    public Type ActualType { get; }

    /// <summary>
    ///     How the types should match.
    /// </summary>
    public MatchingMode Mode { get; }

    /// <inheritdoc />
    public override string Message => BuildMessage();

    private string BuildMessage()
    {
        return Mode switch
        {
            MatchingMode.SameType => $"{PropertyName} should be of type {ExpectedType} but was {ActualType}",
            MatchingMode.AssignableTo => $"{PropertyName} should be assignable to {ExpectedType} but was {ActualType}, which is not",
            _ => throw new ArgumentOutOfRangeException(paramName: nameof(Mode), actualValue: Mode, message: "Invalid enum value")
        };
    }
}

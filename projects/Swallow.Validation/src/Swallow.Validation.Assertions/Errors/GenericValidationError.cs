namespace Swallow.Validation.Errors;

using System.Text;

/// <summary>
///     A generic validation error with a string message.
/// </summary>
/// <remarks>
///     When a certain value does not pass a validation, it produces an error
///     containing the name of the value and a description of the desired state.
/// </remarks>
public sealed class GenericValidationError : ValidationError
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="GenericValidationError" /> class.
    /// </summary>
    /// <param name="requiredState">A description of the required state, e.g. "be less than 10".</param>
    public GenericValidationError(string requiredState)
    {
        RequiredState = requiredState;
    }

    /// <summary>Gets description of the required state.</summary>
    public string RequiredState { get; }

    /// <inheritdoc />
    public override string Message => BuildMessage();

    private string BuildMessage()
    {
        var messageBuilder = new StringBuilder();
        messageBuilder.Append(PropertyName);
        messageBuilder.Append($" must {RequiredState ?? "satisfy predicate"}");
        messageBuilder.Append($" but was '{ActualValue}'");

        return messageBuilder.ToString();
    }
}

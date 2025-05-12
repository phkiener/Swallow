namespace Swallow.Validation.Errors;

/// <summary>
///     A validation error in case of a string not matching a regex.
/// </summary>
public sealed class RegexValidationError : ValidationError
{
    /// <summary>
    ///     Creates an instance of the <see cref="RegexValidationError" /> class.
    /// </summary>
    /// <param name="testedRegex">The regex that was tested with.</param>
    /// <param name="targetDescription">A description of the object whose format is checked.</param>
    /// <remarks>
    ///     The <c>targetDescription</c> will be used in place of the regex to describe the validation.
    ///     A message like <c>"foo must be a valid bar but was 'quux'"</c> will be rendered, where
    ///     bar is the <c>targetDescription</c>.
    /// </remarks>
    public RegexValidationError(string testedRegex, string targetDescription = null)
    {
        TestedRegex = testedRegex;
        TargetDescription = targetDescription;
    }

    /// <summary>
    ///     Gets the regex that was used for validation.
    /// </summary>
    public string TestedRegex { get; }

    /// <summary>
    ///     Get the target description.
    /// </summary>
    public string TargetDescription { get; }

    /// <inheritdoc />
    public override string Message => GetMessage();

    private string GetMessage()
    {
        return this switch
        {
            { TargetDescription: not null } => $"{PropertyName} must be a valid {TargetDescription} but was '{ActualValue}'",
            { TestedRegex: not null } => $"{PropertyName} must match '{TestedRegex}' but was '{ActualValue}'",
            _ => $"{PropertyName} did not match the expected format"
        };
    }
}

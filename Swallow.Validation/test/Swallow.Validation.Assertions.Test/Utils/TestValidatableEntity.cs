namespace Swallow.Validation.Utils;

using TestUtils;

internal class TestValidatableEntity : IValidatable
{
    public bool ShouldSucceed { get; init; } = true;

    public ValidationResult Validate()
    {
        return Validator.Check()
            .That(value: ShouldSucceed, name: "value")
            .Satisfies(predicate: x => x, errorFunc: _ => new TestValidationError())
            .Result();
    }
}

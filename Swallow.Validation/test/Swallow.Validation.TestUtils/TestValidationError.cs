namespace Swallow.Validation.TestUtils;

using Errors;

public sealed class TestValidationError : ValidationError
{
    public override string Message => "An error";
}

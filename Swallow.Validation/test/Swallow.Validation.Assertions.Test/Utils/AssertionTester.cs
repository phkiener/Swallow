namespace Swallow.Validation.Utils;

using System;

internal static class AssertionTester
{
    public static ValidationResult Assert<T>(T value, Func<IAssertable<T>, IAssertion<T>> assertion)
    {
        var validator = Validator.Check().That(value);

        return assertion.Invoke(validator).Result();
    }
}

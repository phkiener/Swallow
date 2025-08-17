using System.Diagnostics.CodeAnalysis;
using Swallow.Validation.Errors;
using Swallow.Validation.TestUtils;

namespace Swallow.Validation.Utils;

using System;

internal static class AssertionTester
{
    public static ValidationResult Assert<T>(T value, Func<IAssertable<T>, IAssertion<T>> assertion)
    {
        var validator = Validator.Check().That(value);

        return assertion.Invoke(validator).Result();
    }

    #nullable enable
    public static bool Assert<T>(T value, IAsserter<T> asserter, [NotNullWhen(false)] out ValidationError? error)
    {
        var valueProvider = TestValue.Of(value);
        if (asserter.Check(valueProvider, out var reportedError))
        {
            error = null;
            return true;
        }

        error = reportedError;
        error.PropertyName = "value";

        return false;
    }
}

using System.Diagnostics.CodeAnalysis;

namespace Swallow.Validation.Next.Internal;

internal abstract class ValidationRule
{
    public abstract string Name { get; }

    public abstract bool IsSatisfied([NotNullWhen(false)] out ValidationError? error);
}

internal sealed class ValidationRule<T>(string name, ValueProvider<T> valueProvider, IAsserter<T> asserter) : ValidationRule
{
    public override string Name { get; } = name;

    public override bool IsSatisfied([NotNullWhen(false)] out ValidationError? error)
    {
        if (asserter.IsValid(valueProvider.Value))
        {
            error = null;
            return true;
        }

        error = asserter.Error;
        return false;
    }
}

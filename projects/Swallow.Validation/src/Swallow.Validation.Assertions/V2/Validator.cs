#nullable enable
using System.Runtime.CompilerServices;

namespace Swallow.Validation.V2;

/// <summary>
/// How the <see cref="Validator"/> should behave when an asserter reports an error.
/// </summary>
public enum ErrorHandling
{
    /// <summary>
    /// Keep evaluating all asserters.
    /// </summary>
    Continue,

    /// <summary>
    /// Skip all asserters for the current property, but continue as normal with the next property.
    /// </summary>
    SkipSameName,

    /// <summary>
    /// Skip all remaining asserters
    /// </summary>
    SkipAll
}

/// <summary>
/// A validator to combine one (or multiple) <see cref="IAsserter{T}"/>s.
/// </summary>
public sealed class Validator
{
    private readonly ErrorHandling errorHandling;
    private readonly List<ValidationRule> validationRules = [];

    private Validator(ErrorHandling errorHandling)
    {
        this.errorHandling = errorHandling;
    }

    /// <summary>
    /// Register an assertion with this validator.
    /// </summary>
    /// <param name="value">The value to assert.</param>
    /// <param name="asserter">The asserter to execute.</param>
    /// <param name="name">Name of the asserted value.</param>
    /// <typeparam name="T">Type of the asserted value.</typeparam>
    public Validator That<T>(T value, IAsserter<T> asserter, [CallerArgumentExpression(nameof(value))] string name = "")
    {
        validationRules.Add(new ValidationRule<T>(name, ValueProvider.Capture(value), asserter));
        return this;
    }

    /// <summary>
    /// Register an assertion with this validator.
    /// </summary>
    /// <param name="value">Function yielding the value to assert.</param>
    /// <param name="asserter">The asserter to execute.</param>
    /// <param name="name">Name of the asserted value.</param>
    /// <typeparam name="T">Type of the asserted value.</typeparam>
    public Validator That<T>(Func<T> value, IAsserter<T> asserter, [CallerArgumentExpression(nameof(value))] string name = "")
    {
        validationRules.Add(new ValidationRule<T>(name, ValueProvider.Function(value), asserter));
        return this;
    }

    /// <summary>
    /// Evaluates all assertions, throwing a <see cref="ValidationFailedException"/> if any of them fail.
    /// </summary>
    public void OrThrow()
    {
        var errors = new List<ValidationError>();
        var failingValues = new HashSet<string>();

        foreach (var rule in validationRules)
        {
            if (errorHandling is ErrorHandling.SkipSameName && failingValues.Contains(rule.Name))
            {
                continue;
            }

            if (!rule.IsSatisfied(out var error))
            {
                failingValues.Add(rule.Name);
                errors.Add(error);

                if (errorHandling is ErrorHandling.SkipAll)
                {
                    break;
                }
            }
        }

        if (errors.Any())
        {
            throw new ValidationFailedException(errors);
        }
    }

    /// <summary>
    /// Create a new <see cref="Validator"/>.
    /// </summary>
    public static Validator Check(ErrorHandling errorHandling = ErrorHandling.SkipSameName)
    {
        return new Validator(errorHandling);
    }
}

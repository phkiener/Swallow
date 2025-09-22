namespace Swallow.Validation.Next.Asserters.Comparison;

/// <summary>
/// How to match a value to a bound.
/// </summary>
public enum BoundsType
{
    /// <summary>
    /// The boundary value itself is <em>not</em> a valid value.
    /// </summary>
    Exclusive,

    /// <summary>
    /// The boundary value itself <em>is</em> a valid value.
    /// </summary>
    Inclusive
}

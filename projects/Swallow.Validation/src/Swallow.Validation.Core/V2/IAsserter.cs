namespace Swallow.Validation.V2;

/// <summary>
/// An asserter to check if a value is satisfying a certain constraint.
/// </summary>
/// <typeparam name="T">Type of the checked value.</typeparam>
public interface IAsserter<in T>
{
    /// <summary>
    /// Check whether the given <paramref name="value"/> is valid.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns><c>true</c> if the value is valid, <c>false</c> otherwise.</returns>
    public bool IsValid(T value);

    /// <summary>
    /// Return a <see cref="ValidationError"/> that describes the constraint this
    /// asserter checks.
    /// </summary>
    public ValidationError Error { get; }
}

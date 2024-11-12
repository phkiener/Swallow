namespace Swallow.Validation.Internal;

internal class StoredValue<T> : IValueProvider<T>
{
    public StoredValue(T value)
    {
        Value = value;
    }

    public T Value { get; }
}

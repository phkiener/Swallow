namespace Swallow.Validation.Internal;

internal sealed class NamedStoredValue<T> : StoredValue<T>, INamedValueProvider<T>
{
    public NamedStoredValue(T value, string name) : base(value)
    {
        Name = name;
    }

    public string Name { get; }
}

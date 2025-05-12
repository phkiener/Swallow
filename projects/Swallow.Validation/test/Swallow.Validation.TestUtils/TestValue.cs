namespace Swallow.Validation.TestUtils;

public class TestValue<T> : INamedValueProvider<T>
{
    public TestValue(T value, string name)
    {
        Value = value;
        Name = name;
    }

    public T Value { get; }
    public string Name { get; }
}

public static class TestValue
{
    public static TestValue<T> Of<T>(T value)
    {
        return new(value: value, name: "value");
    }

    public static TestValue<T> Of<T>(T value, string name)
    {
        return new(value: value, name: name);
    }
}

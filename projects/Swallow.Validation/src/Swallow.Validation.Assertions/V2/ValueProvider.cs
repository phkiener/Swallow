#nullable enable
namespace Swallow.Validation.V2;

internal static class ValueProvider
{
    public static ValueProvider<T> Capture<T>(T value) => new ValueProvider<T>.Captured(value);
    public static ValueProvider<T> Function<T>(Func<T> function) => new ValueProvider<T>.Function(function);
}

internal abstract class ValueProvider<T>
{
    public abstract T Value { get; }

    public sealed class Captured(T value) : ValueProvider<T>
    {
        public override T Value => value;
    }

    public sealed class Function(Func<T> function) : ValueProvider<T>
    {
        public override T Value => function.Invoke();
    }
}

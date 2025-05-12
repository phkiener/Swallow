namespace Swallow.Flux.Default;

public sealed class DefaultBinder(IEmitter emitter) : IBinder, IDisposable
{
    private readonly List<IDisposable> bindings = [];

    public ITargetedBinding<T> Bind<T>(T target) where T : class
    {
        var binding = new TargetedBinding<T>(target, null, emitter);
        bindings.Add(binding);

        return binding;
    }

    public ITargetedBinding<T> Bind<T>(T target, Action<Action> wrapper) where T : class
    {
        var binding = new TargetedBinding<T>(target, wrapper, emitter);
        bindings.Add(binding);

        return binding;
    }

    public void Dispose()
    {
        foreach (var binding in bindings)
        {
            binding.Dispose();
        }
    }
}

namespace Swallow.Flux.Default;

internal sealed class TargetedBinding<T> : ITargetedBinding<T>, IDisposable where T : class
{
    private readonly T target;
    private readonly Action<Action>? wrapper;
    private readonly IEmitter emitter;
    private readonly List<(Type Type, Action<T> Reaction)> subscriptions = [];

    public TargetedBinding(T target, Action<Action>? wrapper, IEmitter emitter)
    {
        this.target = target;
        this.wrapper = wrapper;
        this.emitter = emitter;

        emitter.OnEmit += InvokeSubscribers;
    }

    public ITargetedBinding<T> To<TNotification>(Action<T> reaction) where TNotification : INotification
    {
        subscriptions.Add((typeof(TNotification), reaction));

        return this;
    }

    public void Dispose()
    {
        emitter.OnEmit -= InvokeSubscribers;
    }

    private void InvokeSubscribers(object? sender, INotification e)
    {
        foreach (var subscription in subscriptions.Where(t => t.Type == e.GetType()))
        {
            Invoke(subscription.Reaction);
        }
    }

    private void Invoke(Action<T> reaction)
    {
        if (wrapper is not null)
        {
            wrapper.Invoke(() => reaction.Invoke(target));
        }
        else
        {
            reaction.Invoke(target);
        }
    }
}

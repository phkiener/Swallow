namespace Swallow.Flux.Default;

public sealed class DefaultEmitter : IEmitter
{
    public void Emit(INotification notification)
    {
        OnEmit?.Invoke(this, notification);
    }

    public event EventHandler<INotification>? OnEmit;
}

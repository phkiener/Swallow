namespace Swallow.Flux;

public interface INotification;

public interface IEmitter
{
    void Emit(INotification notification);

    void Emit<T>() where T : INotification, new() => Emit(new T());

    event EventHandler<INotification>? OnEmit;
}

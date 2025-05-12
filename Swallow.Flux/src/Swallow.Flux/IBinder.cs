namespace Swallow.Flux;

public interface ITargetedBinding<out T> where T : class
{
    ITargetedBinding<T> To<TNotification>(Action<T> reaction) where TNotification : INotification;
}

public interface IBinder
{
    ITargetedBinding<T> Bind<T>(T target) where T : class;

    ITargetedBinding<T> Bind<T>(T target, Action<Action> wrapper) where T : class;
}

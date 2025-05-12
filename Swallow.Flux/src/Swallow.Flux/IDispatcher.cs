namespace Swallow.Flux;

public interface IDispatcher
{
    Task Invoke(ICommand command);

    Task Invoke<T>() where T : ICommand, new() => Invoke(new T());
}

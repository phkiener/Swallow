namespace Swallow.Flux.Default;

public sealed class DefaultDispatcher(IEnumerable<IStore> stores) : IDispatcher
{
    public async Task Invoke(ICommand command)
    {
        foreach (var store in stores)
        {
            try
            {
                await store.Handle(command);
            }
            catch (Exception e)
            {
                await Console.Error.WriteLineAsync($"Error in store {store.GetType().Name}: {e}");
            }
        }
    }
}

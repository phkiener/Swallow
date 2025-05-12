using NUnit.Framework;

namespace Swallow.Flux.Default;

[TestFixture]
public sealed class DefaultDispatcherTest
{
    [Test]
    public void NoStores_DoesNothing()
    {
        var dispatcher = new DefaultDispatcher([]);
        Assert.DoesNotThrowAsync(() => dispatcher.Dispatch(new DummyCommand()));
    }

    [Test]
    public async Task MultipleStores_InvokesStoresInOrder()
    {
        var firstStore = new DummyStore();
        var secondStore = new DummyStore();

        var dispatcher = new DefaultDispatcher([firstStore, secondStore]);
        await dispatcher.Dispatch(new DummyCommand());

        Assert.That(firstStore.LastCommand, Is.Not.Default);
        Assert.That(secondStore.LastCommand, Is.Not.Default);
        Assert.That(secondStore.LastCommand, Is.GreaterThan(firstStore.LastCommand));
    }

    [Test]
    public void FirstStoreThrowsException_StillInvokesSecondStore()
    {
        var firstStore = new ThrowingStore();
        var secondStore = new DummyStore();

        var dispatcher = new DefaultDispatcher([firstStore, secondStore]);
        var exception = Assert.ThrowsAsync<AggregateException>(() => dispatcher.Dispatch(new DummyCommand()));

        Assert.That(secondStore.LastCommand, Is.Not.Default);
        Assert.That(exception.InnerExceptions, Has.Count.EqualTo(1));
        Assert.That(exception.InnerExceptions.Single(), Is.InstanceOf<InvalidOperationException>().With.Message.EqualTo("Doesn't work"));
    }

    private sealed record DummyCommand : ICommand;

    private sealed class DummyStore : IStore
    {
        public DateTime LastCommand { get; private set; }

        public async Task Handle(ICommand command, CancellationToken cancellationToken = default)
        {
            LastCommand = DateTime.UtcNow;
            await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken); // to ensure the time comparison works
        }
    }

    private sealed class ThrowingStore : IStore
    {
        public Task Handle(ICommand command, CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("Doesn't work");
        }
    }
}

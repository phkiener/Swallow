using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Swallow.Flux;

[TestFixture]
public sealed class ServiceCollectionExtensionsTest
{
    [Test]
    public void RegistersAllServices()
    {
        using var serviceProvider = new ServiceCollection().AddFlux().BuildServiceProvider();

        Assert.That(serviceProvider.GetService<IDispatcher>(), Is.Not.Null);
        Assert.That(serviceProvider.GetService<IEmitter>(), Is.Not.Null);
        Assert.That(serviceProvider.GetService<IBinder>(), Is.Not.Null);
    }

    [Test]
    public void ExistingRegistrationsAreRespected()
    {
        using var serviceProvider = new ServiceCollection()
            .AddSingleton<IDispatcher, DummyDispatcher>()
            .AddSingleton<IEmitter, DummyEmitter>()
            .AddSingleton<IBinder, DummyBinder>()
            .AddFlux()
            .BuildServiceProvider();

        Assert.That(serviceProvider.GetService<IDispatcher>(), Is.InstanceOf<DummyDispatcher>());
        Assert.That(serviceProvider.GetService<IEmitter>(), Is.InstanceOf<DummyEmitter>());
        Assert.That(serviceProvider.GetService<IBinder>(), Is.InstanceOf<DummyBinder>());
    }

    [Test]
    public async Task ConcreteStoreCanBeRegistered()
    {
        await using var serviceProvider = new ServiceCollection()
            .AddFlux()
            .AddStore<DummyStore>()
            .BuildServiceProvider();

        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
        await dispatcher.Dispatch<DummyCommand>();

        var store = serviceProvider.GetRequiredService<DummyStore>();
        Assert.That(store.CommandsReceived, Is.EqualTo(1));
    }

    [Test]
    public async Task StoreWithInterfaceCanBeRegistered()
    {
        await using var serviceProvider = new ServiceCollection()
            .AddFlux()
            .AddStore<IDummyStore, DummyStore>()
            .BuildServiceProvider();

        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
        await dispatcher.Dispatch<DummyCommand>();

        var store = serviceProvider.GetRequiredService<IDummyStore>();
        Assert.That(store.CommandsReceived, Is.EqualTo(1));
    }

    private sealed class DummyDispatcher : IDispatcher
    {
        public Task Dispatch(ICommand command, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }

    private sealed class DummyEmitter : IEmitter
    {
        public void Emit(INotification notification) => throw new NotImplementedException();

#pragma warning disable CS0067 // It's unused because it's just a dummy
        public event EventHandler<INotification>? OnEmit;
    }

    private sealed class DummyBinder : IBinder
    {
        public ITargetedBinding<T> Bind<T>(T target) where T : class => throw new NotImplementedException();

        public ITargetedBinding<T> Bind<T>(T target, Action<Action> wrapper) where T : class => throw new NotImplementedException();

        public void Dispose() { }
    }

    private sealed record DummyCommand : ICommand;

    private interface IDummyStore : IStore
    {
        public int CommandsReceived { get; }
    }

    private sealed class DummyStore : AbstractStore, IDummyStore
    {
        public DummyStore(IEmitter emitter) : base(emitter)
        {
            Register<DummyCommand>(() => CommandsReceived += 1);
        }

        public int CommandsReceived { get; private set; } = 0;
    }
}

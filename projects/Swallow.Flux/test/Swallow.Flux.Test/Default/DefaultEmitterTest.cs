using NUnit.Framework;

namespace Swallow.Flux.Default;

[TestFixture]
public sealed class DefaultEmitterTest
{
    [Test]
    public void InvokesEventHandler()
    {
        INotification? receivedNotification = null;
        object? receivedSender = null;

        var emitter = new DefaultEmitter();
        emitter.OnEmit += (sender, notification) =>
        {
            receivedSender = sender;
            receivedNotification = notification;
        };

        emitter.Emit(new DummyNotification());

        Assert.That(receivedSender, Is.SameAs(emitter));
        Assert.That(receivedNotification, Is.Not.Null.And.TypeOf<DummyNotification>());
    }

    private sealed record DummyNotification : INotification;
}

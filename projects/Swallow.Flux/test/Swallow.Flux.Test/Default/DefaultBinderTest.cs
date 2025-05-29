using NUnit.Framework;

namespace Swallow.Flux.Default;

[TestFixture]
public sealed class DefaultBinderTest
{
    [Test]
    public void InvokesBoundReaction_OnRelevantNotification()
    {
        var emitter = new DefaultEmitter();
        var binder = new DefaultBinder(emitter);

        const string target = "test";
        string receivedText = "";
        binder.Bind(target).To<RelevantNotification>(t => receivedText += t);

        emitter.Emit(new RelevantNotification());
        Assert.That(receivedText, Is.EqualTo(target));
    }

    [Test]
    public void DoesNotInvokeReaction_OnIrrelevantNotification()
    {
        var emitter = new DefaultEmitter();
        var binder = new DefaultBinder(emitter);

        const string target = "test";
        string receivedText = "";
        binder.Bind(target).To<RelevantNotification>(t => receivedText += t);

        emitter.Emit(new IrrelevantNotification());
        Assert.That(receivedText, Is.Empty);
    }

    [Test]
    public void InvokesSubscriptionTwice_WhenBoundTwice()
    {
        var emitter = new DefaultEmitter();
        var binder = new DefaultBinder(emitter);

        const string target = "test";
        string receivedText = "";
        binder.Bind(target)
            .To<RelevantNotification>(t => receivedText += t)
            .To<RelevantNotification>(t => receivedText += t);

        emitter.Emit(new RelevantNotification());
        Assert.That(receivedText, Is.EqualTo(target + target));
    }

    [Test]
    public void InvokesAllBoundSubscriptions()
    {
        var emitter = new DefaultEmitter();
        var binder = new DefaultBinder(emitter);

        const string target = "test";
        const string otherTarget = "TEST";
        string receivedText = "";
        binder.Bind(target).To<RelevantNotification>(t => receivedText += t);
        binder.Bind(otherTarget).To<RelevantNotification>(t => receivedText += t);

        emitter.Emit(new RelevantNotification());
        Assert.That(receivedText, Is.EqualTo(target + otherTarget));
    }

    [Test]
    public void InvokesWrapperAroundReaction()
    {
        var emitter = new DefaultEmitter();
        var binder = new DefaultBinder(emitter);

        const string target = "test";
        string receivedText = "";
        binder.Bind(target, act => CatchAndWrite(act, ref receivedText)).To<RelevantNotification>(_ => throw new InvalidOperationException("wrong!"));

        emitter.Emit(new RelevantNotification());
        Assert.That(receivedText, Is.EqualTo("wrong!"));
    }

    [Test]
    public void InvokesReaction_WhenConfigured()
    {
        var emitter = new DefaultEmitter();
        var binder = new DefaultBinder(emitter);

        const string target = "test";
        string receivedText = "";

        binder.Bind(target).To<RelevantNotification>(t => receivedText += t, immediatelyInvoke: true);
        Assert.That(receivedText, Is.EqualTo("test"));
    }

    [Test]
    public void PassesNotificationToReaction()
    {
        var emitter = new DefaultEmitter();
        var binder = new DefaultBinder(emitter);

        const string target = "test";
        ParameterizedNotification? receivedNotification = null;

        binder.Bind(target).To<ParameterizedNotification>((_, n) => receivedNotification = n);

        emitter.Emit(new ParameterizedNotification(Id: 42));
        Assert.That(receivedNotification?.Id, Is.EqualTo(42));
    }

    [Test]
    public void InvokesReactionWithDefaultConstructedNotification_WhenConfigured()
    {
        var emitter = new DefaultEmitter();
        var binder = new DefaultBinder(emitter);

        const string target = "test";
        ParameterizedNotification? receivedNotification = null;

        binder.Bind(target).To<ParameterizedNotification>((_, n) => receivedNotification = n, immediatelyInvoke: true);

        Assert.That(receivedNotification?.Id, Is.EqualTo(99));
    }

    [Test]
    public void InvokesWrapper_WhenImmediatelyInvoking()
    {
        var emitter = new DefaultEmitter();
        var binder = new DefaultBinder(emitter);

        const string target = "test";
        int wrapperCalls = 0;

        binder.Bind(target, act => { wrapperCalls += 1; act.Invoke(); })
            .To<RelevantNotification>(_ => { }, immediatelyInvoke: true)
            .To<RelevantNotification>((_, _) => { }, immediatelyInvoke: true);

        Assert.That(wrapperCalls, Is.EqualTo(2));
    }

    private static void CatchAndWrite(Action action, ref string output)
    {
        try
        {
            action.Invoke();
        }
        catch (Exception e)
        {
            output = e.Message;
        }
    }

    private sealed record RelevantNotification : INotification;
    private sealed record IrrelevantNotification : INotification;

    private sealed record ParameterizedNotification(int Id) : INotification
    {
        public ParameterizedNotification() : this(99) { }
    }
}

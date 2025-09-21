namespace Swallow.ChainOfInjection.Test.SimpleInjector;

using System;
using ChainOfInjection.SimpleInjector;
using global::SimpleInjector;
using global::SimpleInjector.Lifestyles;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

[TestFixture]
internal sealed class SimpleInjectorChainConfigurationShould
{
    private Container container;

    [SetUp]
    public void SetUp()
    {
        container = new();
        container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
    }

    [Test]
    public void ThrowException_WhenNoChainMembersHaveBeenConfigured()
    {
        var chainConfiguration = container.RegisterChain<IChainMember>();

        var exception = Assert.Throws<InvalidOperationException>(() => chainConfiguration.Configure());
        Assert.That(exception?.Message, Is.EqualTo($"No implementation defined for {typeof(IChainMember)}. Please add one (or more) using '{nameof(chainConfiguration.Add)}'."));
    }

    [Test]
    public void RegisterTypeWithCorrectLifestyle()
    {
        var chainConfiguration = container.RegisterChain<IChainMember>();
        chainConfiguration.Add<TerminatingMember>(Lifestyle.Transient).Configure();

        var registration = container.GetRegistration<IChainMember>();
        using var scope = AsyncScopedLifestyle.BeginScope(container);
        var service = container.GetService<IChainMember>();

        Assert.That(registration?.Lifestyle, Is.EqualTo(Lifestyle.Transient));
        Assert.That(service, Is.InstanceOf<TerminatingMember>());
    }

    [Test]
    public void RegisterChainMembersWithGivenDefaultLifestyle()
    {
        var chainConfiguration = container.RegisterChain<IChainMember>(Lifestyle.Singleton);
        chainConfiguration.Add<ChainingMember>().Add<TerminatingMember>().Configure();

        Assert.That(container.GetRegistration<IChainMember>()!.Lifestyle, Is.EqualTo(Lifestyle.Singleton));
        Assert.That(container.GetRegistration<TerminatingMember>()!.Lifestyle, Is.EqualTo(Lifestyle.Singleton));
    }
}

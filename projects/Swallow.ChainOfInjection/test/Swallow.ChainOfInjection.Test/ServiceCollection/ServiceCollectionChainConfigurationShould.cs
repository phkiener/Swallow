namespace Swallow.ChainOfInjection.Test.ServiceCollection;

using System;
using System.Linq;
using ChainOfInjection.ServiceCollection;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

[TestFixture]
internal sealed class ServiceCollectionChainConfigurationShould
{
    private IServiceCollection serviceCollection;

    [SetUp]
    public void SetUp()
    {
        serviceCollection = new ServiceCollection();
    }

    [Test]
    public void ThrowException_WhenNoChainMembersHaveBeenConfigured()
    {
        var chainConfiguration = serviceCollection.AddChain<IChainMember>();

        var exception = Assert.Throws<InvalidOperationException>(() => chainConfiguration.Configure());
        Assert.That(exception?.Message, Is.EqualTo($"No implementation defined for {typeof(IChainMember)}. Please add one (or more) using '{nameof(chainConfiguration.Add)}'."));
    }

    [Test]
    public void RegisterTypeWithCorrectLifestyle()
    {
        var chainConfiguration = serviceCollection.AddChain<IChainMember>();
        chainConfiguration.Add<TerminatingMember>(ServiceLifetime.Transient).Configure();

        var serviceDescriptor = serviceCollection.Single();
        var service = serviceCollection.BuildServiceProvider().GetService<IChainMember>();

        Assert.That(serviceDescriptor.ServiceType, Is.EqualTo(typeof(IChainMember)));
        Assert.That(serviceDescriptor.Lifetime, Is.EqualTo(ServiceLifetime.Transient));
        Assert.That(service, Is.InstanceOf<TerminatingMember>());
    }

    [Test]
    public void RegisterChainMembersWithGivenDefaultLifestyle()
    {
        var chainConfiguration = serviceCollection.AddChain<IChainMember>(ServiceLifetime.Singleton);
        chainConfiguration.Add<ChainingMember>().Add<TerminatingMember>().Configure();

        Assert.That(serviceCollection, Has.All.Matches<ServiceDescriptor>(sd => sd.Lifetime == ServiceLifetime.Singleton));
    }
}

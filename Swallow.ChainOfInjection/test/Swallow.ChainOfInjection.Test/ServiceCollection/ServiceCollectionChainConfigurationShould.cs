namespace Swallow.ChainOfInjection.Test.ServiceCollection;

using System;
using System.Linq;
using ChainOfInjection.ServiceCollection;
using FluentAssertions;
using FluentAssertions.Execution;
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
        // Arrange
        var chainConfiguration = serviceCollection.AddChain<IChainMember>();

        // Act
        var act = () => chainConfiguration.Configure();

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"No implementation defined for {typeof(IChainMember)}. Please add one (or more) using '{nameof(chainConfiguration.Add)}'.");
    }

    [Test]
    public void RegisterTypeWithCorrectLifestyle()
    {
        // Arrange
        var chainConfiguration = serviceCollection.AddChain<IChainMember>();

        // Act
        chainConfiguration.Add<TerminatingMember>(ServiceLifetime.Transient).Configure();

        // Assert
        var serviceDescriptor = serviceCollection.Single();
        using (new AssertionScope())
        {
            serviceDescriptor.ServiceType.Should().Be<IChainMember>();
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Transient);
        }

        var service = serviceCollection.BuildServiceProvider().GetService<IChainMember>();
        service.Should().BeOfType<TerminatingMember>();
    }

    [Test]
    public void RegisterChainMembersWithGivenDefaultLifestyle()
    {
        // Arrange
        var chainConfiguration = serviceCollection.AddChain<IChainMember>(ServiceLifetime.Singleton);

        // Act
        chainConfiguration.Add<ChainingMember>().Add<TerminatingMember>().Configure();

        // Assert
        serviceCollection.Should().OnlyContain(sd => sd.Lifetime == ServiceLifetime.Singleton);
        var service = serviceCollection.BuildServiceProvider().GetService<IChainMember>();
        service.Should().BeOfType<ChainingMember>();
        service.As<ChainingMember>().Next.Should().BeOfType<TerminatingMember>();
    }
}


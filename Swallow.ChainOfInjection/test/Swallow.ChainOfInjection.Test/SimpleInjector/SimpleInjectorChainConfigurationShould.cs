namespace Swallow.ChainOfInjection.Test.SimpleInjector;

using System;
using ChainOfInjection.SimpleInjector;
using FluentAssertions;
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
        // Arrange
        var chainConfiguration = container.RegisterChain<IChainMember>();

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
        var chainConfiguration = container.RegisterChain<IChainMember>();

        // Act
        chainConfiguration.Add<TerminatingMember>(Lifestyle.Transient).Configure();

        // Assert
        var registration = container.GetRegistration<IChainMember>();
        registration!.Lifestyle.Should().Be(Lifestyle.Transient);
        using var scope = AsyncScopedLifestyle.BeginScope(container);
        var service = container.GetService<IChainMember>();
        service.Should().BeOfType<TerminatingMember>();
    }

    [Test]
    public void RegisterChainMembersWithGivenDefaultLifestyle()
    {
        // Arrange
        var chainConfiguration = container.RegisterChain<IChainMember>(Lifestyle.Singleton);

        // Act
        chainConfiguration.Add<ChainingMember>().Add<TerminatingMember>().Configure();

        // Assert
        container.GetRegistration<IChainMember>()!.Lifestyle.Should().Be(Lifestyle.Singleton);
        container.GetRegistration<TerminatingMember>()!.Lifestyle.Should().Be(Lifestyle.Singleton);
        using var scope = AsyncScopedLifestyle.BeginScope(container);
        var service = container.GetService<IChainMember>();
        service.Should().BeOfType<ChainingMember>();
        service.As<ChainingMember>().Next.Should().BeOfType<TerminatingMember>();
    }
}

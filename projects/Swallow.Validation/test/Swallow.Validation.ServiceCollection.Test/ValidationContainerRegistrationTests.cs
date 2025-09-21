namespace Swallow.Validation.ServiceCollection;

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

[TestFixture]
public sealed class ValidationContainerRegistrationTests
{
    [Test]
    public void RegisterAllFoundAssertersAsSingletons()
    {
        var serviceCollection = new ServiceCollection().AddValidationContainer(Assembly.GetExecutingAssembly());

        Assert.That(serviceCollection, Has.One.Matches<ServiceDescriptor>(static sd => sd.Lifetime == ServiceLifetime.Singleton && sd.ServiceType == typeof(ObjectAsserter)));
        Assert.That(serviceCollection, Has.One.Matches<ServiceDescriptor>(static sd => sd.Lifetime == ServiceLifetime.Singleton && sd.ServiceType == typeof(IntAsserter)));
        Assert.That(serviceCollection, Has.One.Matches<ServiceDescriptor>(static sd => sd.Lifetime == ServiceLifetime.Singleton && sd.ServiceType == typeof(StringAsserter)));
        Assert.That(serviceCollection, Has.One.Matches<ServiceDescriptor>(static sd => sd.Lifetime == ServiceLifetime.Singleton && sd.ServiceType == typeof(AnotherStringAsserter)));
    }

    [Test]
    public void NotRegisterGenericType()
    {
        var serviceCollection = new ServiceCollection().AddValidationContainer(Assembly.GetExecutingAssembly());

        Assert.That(serviceCollection, Has.None.Matches<ServiceDescriptor>(static sd => sd.ServiceType == typeof(AsserterWithGeneric<>)));
    }

    [Test]
    public void NotRegisterAbstractType()
    {
        var serviceCollection = new ServiceCollection().AddValidationContainer(Assembly.GetExecutingAssembly());

        Assert.That(serviceCollection, Has.None.Matches<ServiceDescriptor>(static sd => sd.ServiceType == typeof(TestAsserterBase<>)));
    }

    [Test]
    public void RegisterTheValidationContainer()
    {
        // Arrange
        var serviceCollection = new ServiceCollection().AddValidationContainer(Assembly.GetExecutingAssembly());

        var validationContainer = serviceCollection.BuildServiceProvider().GetService<ValidationContainer>();
        Assert.That(validationContainer, Is.Not.Null);
    }
}

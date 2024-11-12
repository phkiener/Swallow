namespace Swallow.Validation.ServiceCollection;

using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

[TestFixture]
public sealed class ServiceProviderConfigShould
{
    [Test]
    public void RegisterAllFoundAssertersAsSingletons()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();

        // Act
        serviceCollection.AddValidationContainer(Assembly.GetExecutingAssembly());

        // Assert
        serviceCollection.Should().Contain(sd => sd.Lifetime == ServiceLifetime.Singleton && sd.ServiceType == typeof(ObjectAsserter));
        serviceCollection.Should().Contain(sd => sd.Lifetime == ServiceLifetime.Singleton && sd.ServiceType == typeof(IntAsserter));
        serviceCollection.Should().Contain(sd => sd.Lifetime == ServiceLifetime.Singleton && sd.ServiceType == typeof(StringAsserter));
        serviceCollection.Should().Contain(sd => sd.Lifetime == ServiceLifetime.Singleton && sd.ServiceType == typeof(AnotherStringAsserter));
    }

    [Test]
    public void NotRegisterGenericType()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();

        // Act
        serviceCollection.AddValidationContainer(Assembly.GetExecutingAssembly());

        // Assert
        serviceCollection.Should().NotContain(sd => sd.ServiceType == typeof(AsserterWithGeneric<>));
    }

    [Test]
    public void NotRegisterAbstractType()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();

        // Act
        serviceCollection.AddValidationContainer(Assembly.GetExecutingAssembly());

        // Assert
        serviceCollection.Should().NotContain(sd => sd.ServiceType == typeof(TestAsserterBase<>));
    }

    [Test]
    public void RegisterTheValidationContainer()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();

        // Act
        serviceCollection.AddValidationContainer(Assembly.GetExecutingAssembly());
        var validationContainer = serviceCollection.BuildServiceProvider().GetService<ValidationContainer>();

        // Assert
        serviceCollection.Should().Contain(sd => sd.ServiceType == typeof(ValidationContainer));
        validationContainer.Should().NotBeNull();
    }
}

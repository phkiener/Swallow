namespace Swallow.ChainOfInjection.Test;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;

[TestFixture]
public class AbstractChainConfiguratorTest
{
    private TestChainConfigurator testChainConfigurator;

    [SetUp]
    public void SetUp()
    {
        testChainConfigurator = new();
    }

    [Test]
    public void Configure_NoMemberHasBeenAdded_ThrowsException()
    {
        // Act
        var act = () => testChainConfigurator.Configure();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void Configure_SingleChainingTypeAdded_RegistersTypeWithCorrectLifestyle()
    {
        // Act
        const int lifestyle = 1;
        testChainConfigurator.Add<ChainingMember>(lifestyle).Configure();

        // Assert
        var registeredType = testChainConfigurator.GetRegistrationFor<IChainMember>();
        registeredType.Lifestyle.Should().Be(lifestyle);
    }

    [Test]
    public void Configure_SingleChainingTypeAdded_RegistersTypeAsFirstMemberWithNextAsNull()
    {
        // Act
        testChainConfigurator.Add<ChainingMember>().Configure();

        // Assert
        var registeredObject = testChainConfigurator.Get<IChainMember>() as ChainingMember;
        registeredObject.Should().NotBeNull();
        registeredObject!.Next.Should().BeNull();
    }

    [Test]
    public void Configure_ChainingTypeAndTerminatingTypeAdded_RegistersChainingTypeAsFirstMemberWithNextAsTerminatingType()
    {
        // Act
        testChainConfigurator.Add<ChainingMember>().Add<TerminatingMember>().Configure();

        // Assert
        var registeredObject = testChainConfigurator.Get<IChainMember>() as ChainingMember;
        registeredObject.Should().NotBeNull();
        registeredObject!.Next.Should().BeOfType<TerminatingMember>();
    }

    [Test]
    public void Configure_TerminatingTypeAndChainingAdded_RegistersTerminatingType()
    {
        // Act
        testChainConfigurator.Add<TerminatingMember>().Add<ChainingMember>().Configure();

        // Assert
        testChainConfigurator.Get<IChainMember>().Should().BeOfType<TerminatingMember>();
    }

    [Test]
    public void Configure_TerminatingTypeGivenAsParameter_RegistersTerminatingType()
    {
        // Act
        testChainConfigurator.Add(typeof(TerminatingMember)).Configure();

        // Assert
        testChainConfigurator.Get<IChainMember>().Should().BeOfType<TerminatingMember>();
    }

    [Test]
    public void Configure_TypeGivenThatHasNoPublicConstructor_ThrowsException()
    {
        // Act
        var act = () => testChainConfigurator.Add(typeof(MemberWithoutPublicConstructor)).Configure();

        // Act & Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void Configure_TypeGivenThatHasMoreThanOnePublicConstructor_ThrowsException()
    {
        // Act
        var act = () => testChainConfigurator.Add(typeof(MemberWithMultipleConstructors)).Configure();

        // Act & Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void Add_TypeGivenThatIsNotAChainMember_ThrowsException()
    {
        // Act
        var act = () => testChainConfigurator.Add(typeof(MemberThatIsNotAChainMember)).Configure();

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Add_TypeThatAlsoRequiresANonChainMemberType_UsesAlreadyExistingRegistrationForOtherType()
    {
        // Arrange
        testChainConfigurator.Register("Hello World!");

        // Act
        testChainConfigurator.Add<MemberThatAlsoRequiresANonMember>().Add<TerminatingMember>().Configure();

        // Assert
        var registeredObject = testChainConfigurator.Get<IChainMember>() as MemberThatAlsoRequiresANonMember;
        registeredObject.Should().NotBeNull();
        registeredObject!.SomeString.Should().Be("Hello World!");
        registeredObject!.Next.Should().BeOfType<TerminatingMember>();
    }

    private sealed class TestChainConfigurator : AbstractChainConfigurator<IChainMember, TestChainConfigurator.Factory, int>
    {
        public delegate object Factory();
        private readonly IDictionary<Type, RegisteredType> registeredTypes = new Dictionary<Type, RegisteredType>();

        public TestChainConfigurator() : base(0) { }

        protected override void Register(Type targetType, Factory factory, int lifestyle)
        {
            registeredTypes[targetType] = new(Lifestyle: lifestyle, GetInstance: factory);
        }

        protected override Factory CreateFactory(ConstructorInfo constructor, IReadOnlyList<Type> parameterTypes)
        {
            return () => constructor.Invoke(parameterTypes.Select(t => t == null ? null : registeredTypes[t].GetInstance()).ToArray());
        }

        public void Register<T>(T value)
        {
            registeredTypes.Add(key: typeof(T), value: new(Lifestyle: 0, GetInstance: () => value));
        }

        public RegisteredType GetRegistrationFor<T>()
        {
            return registeredTypes.TryGetValue(key: typeof(T), value: out var registeredType) ? registeredType : null;
        }

        public T Get<T>()
        {
            return (T)GetRegistrationFor<T>().GetInstance();
        }

        public sealed record RegisteredType(int Lifestyle, Factory GetInstance);
    }
}


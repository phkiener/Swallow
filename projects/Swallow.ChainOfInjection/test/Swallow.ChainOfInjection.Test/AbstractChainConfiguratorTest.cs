namespace Swallow.ChainOfInjection.Test;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        Assert.Throws<InvalidOperationException>(() => testChainConfigurator.Configure());
    }

    [Test]
    public void Configure_SingleChainingTypeAdded_RegistersTypeWithCorrectLifestyle()
    {
        const int lifestyle = 1;
        testChainConfigurator.Add<ChainingMember>(lifestyle).Configure();

        var registeredType = testChainConfigurator.GetRegistrationFor<IChainMember>();
        Assert.That(registeredType.Lifestyle, Is.EqualTo(lifestyle));
    }

    [Test]
    public void Configure_SingleChainingTypeAdded_RegistersTypeAsFirstMemberWithNextAsNull()
    {
        testChainConfigurator.Add<ChainingMember>().Configure();

        var registeredObject = testChainConfigurator.Get<IChainMember>() as ChainingMember;
        Assert.That(registeredObject, Is.Not.Null);
        Assert.That(registeredObject.Next, Is.Null);
    }

    [Test]
    public void Configure_ChainingTypeAndTerminatingTypeAdded_RegistersChainingTypeAsFirstMemberWithNextAsTerminatingType()
    {
        testChainConfigurator.Add<ChainingMember>().Add<TerminatingMember>().Configure();

        var registeredObject = testChainConfigurator.Get<IChainMember>() as ChainingMember;
        Assert.That(registeredObject, Is.Not.Null);
        Assert.That(registeredObject.Next, Is.InstanceOf<TerminatingMember>());
    }

    [Test]
    public void Configure_TerminatingTypeAndChainingAdded_RegistersTerminatingType()
    {
        testChainConfigurator.Add<TerminatingMember>().Add<ChainingMember>().Configure();

        Assert.That(testChainConfigurator.Get<IChainMember>(), Is.InstanceOf<TerminatingMember>());
    }

    [Test]
    public void Configure_TerminatingTypeGivenAsParameter_RegistersTerminatingType()
    {
        testChainConfigurator.Add(typeof(TerminatingMember)).Configure();

        Assert.That(testChainConfigurator.Get<IChainMember>(), Is.InstanceOf<TerminatingMember>());
    }

    [Test]
    public void Configure_TypeGivenThatHasNoPublicConstructor_ThrowsException()
    {
        Assert.Throws<InvalidOperationException>(() => testChainConfigurator.Add(typeof(MemberWithoutPublicConstructor)).Configure());
    }

    [Test]
    public void Configure_TypeGivenThatHasMoreThanOnePublicConstructor_ThrowsException()
    {
        Assert.Throws<InvalidOperationException>(() => testChainConfigurator.Add(typeof(MemberWithMultipleConstructors)).Configure());
    }

    [Test]
    public void Add_TypeGivenThatIsNotAChainMember_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => testChainConfigurator.Add(typeof(MemberThatIsNotAChainMember)).Configure());
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
        Assert.That(registeredObject, Is.Not.Null);
        Assert.That(registeredObject.SomeString, Is.EqualTo("Hello World!"));
        Assert.That(registeredObject.Next, Is.InstanceOf<TerminatingMember>());
    }

    private sealed class TestChainConfigurator() : AbstractChainConfigurator<IChainMember, TestChainConfigurator.Factory, int>(0)
    {
        public delegate object Factory();
        private readonly IDictionary<Type, RegisteredType> registeredTypes = new Dictionary<Type, RegisteredType>();

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

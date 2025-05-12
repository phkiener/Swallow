namespace Swallow.Validation;

using FluentAssertions;
using NUnit.Framework;
using TestUtils;

[TestFixture]
internal class AsserterExtensionsShould
{
    public sealed class OnCheck : AsserterExtensionsShould
    {
        [Test]
        public void SetPropertyNameToValue()
        {
            // Arrange
            var asserter = TestAsserter.Failing<int>();

            // Act
            _ = asserter.Check(value: 5, error: out var error);

            // Assert
            error!.PropertyName.Should().Be("value");
        }

        [Test]
        public void SetPropertyNameToGivenName()
        {
            // Arrange
            var asserter = TestAsserter.Failing<int>();

            // Act
            _ = asserter.Check(value: 5, name: "MyCoolInt", error: out var error);

            // Assert
            error!.PropertyName.Should().Be("MyCoolInt");
        }

        [Test]
        public void SetPropertyNameToNameOfValueProvider()
        {
            // Arrange
            var asserter = TestAsserter.Failing<string>();

            // Act
            var valueProvider = TestValue.Of(value: "Value", name: "Name");
            _ = asserter.Check(valueProvider: valueProvider, error: out var error);

            // Assert
            error!.PropertyName.Should().Be(valueProvider.Name);
        }
    }
}

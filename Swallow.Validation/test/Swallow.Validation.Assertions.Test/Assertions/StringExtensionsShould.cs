namespace Swallow.Validation.Assertions;

using System.Text.RegularExpressions;
using Errors;
using NUnit.Framework;
using Utils;

[TestFixture]
internal class StringExtensionsShould
{
    public sealed class OnIsNotEmpty : StringExtensionsShould
    {
        [Test]
        public void PassAssertion_WhenStringIsNotEmpty()
        {
            // Act
            var result = AssertionTester.Assert(value: "hello world", assertion: v => v.IsNotEmpty());

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void FailAssertion_WhenStringIsEmpty()
        {
            // Act
            var result = AssertionTester.Assert(value: "", assertion: v => v.IsNotEmpty());

            // Assert
            result.Should().HaveError<EmptyCollectionValidationError>();
        }
    }

    public class OnMatchesWithString : StringExtensionsShould
    {
        [Test]
        public void FailAssertion_WhenStringDoesNotMatchRegex()
        {
            // Act
            var result = AssertionTester.Assert(value: "Hello", assertion: v => v.Matches("^[A-Z][0-9]+$"));

            // Assert
            result.Should().HaveError<RegexValidationError>();
        }

        [Test]
        public void PassAssertion_WhenStringMatchesRegex()
        {
            // Act
            var result = AssertionTester.Assert(value: "A123", assertion: v => v.Matches("^[A-Z][0-9]+$"));

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void PassAssertion_WhenStringMatchesRegexWithOptionsGiven()
        {
            // Act
            var result = AssertionTester.Assert(
                value: "a123",
                assertion: v => v.Matches(regex: "^[A-Z][0-9]+$", regexOptions: RegexOptions.IgnoreCase));

            // Assert
            result.Should().BeSuccess();
        }
    }

    public class OnMatchesWithRegex : StringExtensionsShould
    {
        [Test]
        public void FailAssertion_WhenStringDoesNotMatchRegex()
        {
            // Act
            var regex = new Regex("^[A-Z][0-9]+$");
            var result = AssertionTester.Assert(value: "Hello", assertion: v => v.Matches(regex));

            // Assert
            result.Should().HaveError<RegexValidationError>();
        }

        [Test]
        public void PassAssertion_WhenStringMatchesRegex()
        {
            // Act
            var regex = new Regex("^[A-Z][0-9]+$");
            var result = AssertionTester.Assert(value: "A123", assertion: v => v.Matches(regex));

            // Assert
            result.Should().BeSuccess();
        }
    }

    public class OnIsNotOnlyWhitespace : StringExtensionsShould
    {
        [Test]
        public void FailAssertion_WhenStringIsEmpty()
        {
            // Act
            var result = AssertionTester.Assert(value: "", assertion: v => v.IsNotOnlyWhitespace());

            // Assert
            result.Should().HaveError<IsOnlyWhitespaceError>();
        }

        [Test]
        public void FailAssertion_WhenStringIsOnlyWhitespace()
        {
            // Act
            var result = AssertionTester.Assert(value: "   ", assertion: v => v.IsNotOnlyWhitespace());

            // Assert
            result.Should().HaveError<IsOnlyWhitespaceError>();
        }

        [Test]
        public void PassAssertion_WhenStringHasSomeText()
        {
            // Act
            var result = AssertionTester.Assert(value: "Hello, World!", assertion: v => v.IsNotOnlyWhitespace());

            // Assert
            result.Should().BeSuccess();
        }

        [Test]
        public void PassAssertion_WhenStringHasSomeTextEnclosedInWhitespace()
        {
            // Act
            var result = AssertionTester.Assert(value: "    Whitespace!    ", assertion: v => v.IsNotOnlyWhitespace());

            // Assert
            result.Should().BeSuccess();
        }
    }
}

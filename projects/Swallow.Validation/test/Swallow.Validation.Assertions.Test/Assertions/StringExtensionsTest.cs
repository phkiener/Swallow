namespace Swallow.Validation.Assertions;

using System.Text.RegularExpressions;
using Errors;
using NUnit.Framework;
using Utils;

[TestFixture]
public sealed class StringExtensionsTest
{
    [Test]
    public void NotEmpty_PassAssertion_WhenStringIsNotEmpty()
    {
        var result = AssertionTester.Assert(value: "hello world", assertion: v => v.IsNotEmpty());
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void NotEmpty_FailAssertion_WhenStringIsEmpty()
    {
        var result = AssertionTester.Assert(value: "", assertion: v => v.IsNotEmpty());
        Assert.That(result.Errors, Has.One.InstanceOf<EmptyCollectionValidationError>());
    }

    [Test]
    public void MatchesRegexPattern_FailAssertion_WhenStringDoesNotMatchRegex()
    {
        var result = AssertionTester.Assert(value: "Hello", assertion: v => v.Matches("^[A-Z][0-9]+$"));
        Assert.That(result.Errors, Has.One.InstanceOf<RegexValidationError>());
    }

    [Test]
    public void MatchesRegexPattern_PassAssertion_WhenStringMatchesRegex()
    {
        var result = AssertionTester.Assert(value: "A123", assertion: v => v.Matches("^[A-Z][0-9]+$"));
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void MatchesRegex_PassAssertion_WhenStringMatchesRegexWithOptionsGiven()
    {
        var result = AssertionTester.Assert(
            value: "a123",
            assertion: v => v.Matches(regex: "^[A-Z][0-9]+$", regexOptions: RegexOptions.IgnoreCase));

        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void MatchesRegex_FailAssertion_WhenStringDoesNotMatchRegex()
    {
        var regex = new Regex("^[A-Z][0-9]+$");
        var result = AssertionTester.Assert(value: "Hello", assertion: v => v.Matches(regex));

        Assert.That(result.Errors, Has.One.InstanceOf<RegexValidationError>());
    }

    [Test]
    public void MatchesRegex_PassAssertion_WhenStringMatchesRegex()
    {
        var regex = new Regex("^[A-Z][0-9]+$");
        var result = AssertionTester.Assert(value: "A123", assertion: v => v.Matches(regex));

        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void NotOnlyWhitespace_FailAssertion_WhenStringIsEmpty()
    {
        var result = AssertionTester.Assert(value: "", assertion: v => v.IsNotOnlyWhitespace());
        Assert.That(result.Errors, Has.One.InstanceOf<IsOnlyWhitespaceError>());
    }

    [Test]
    public void NotOnlyWhitespace_FailAssertion_WhenStringIsOnlyWhitespace()
    {
        var result = AssertionTester.Assert(value: "   ", assertion: v => v.IsNotOnlyWhitespace());
        Assert.That(result.Errors, Has.One.InstanceOf<IsOnlyWhitespaceError>());
    }

    [Test]
    public void NotOnlyWhitespace_PassAssertion_WhenStringHasSomeText()
    {
        var result = AssertionTester.Assert(value: "Hello, World!", assertion: v => v.IsNotOnlyWhitespace());
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public void NotOnlyWhitespace_PassAssertion_WhenStringHasSomeTextEnclosedInWhitespace()
    {
        var result = AssertionTester.Assert(value: "    Whitespace!    ", assertion: v => v.IsNotOnlyWhitespace());
        Assert.That(result.IsSuccess, Is.True);
    }
}

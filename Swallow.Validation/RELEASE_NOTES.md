# 7.0.0

- Update package and all dependencies for .NET 8

# 6.0.1

- No functional change, just removed the now-obsolete GitLab-pages link from the nuget package properties

# 6.0.0

- Update package and all dependencies for .NET 7
- New logo, yay!

# 5.0.1

- Remove nullability annotations for reference types

# 5.0.0

- Enable nullability annotations for the whole package
- `error`-parameter of `IAsserter<>.Check` is now `[NotNullWhen(false)] out ValidationError? error`
    - The attribute is part of the signature - if you implement that interface, you gotta add the attribute as well
- `IsNotNull`-assertions are allowed for both nullable reference types and nullable value types
    - Most other validations are only allowed for non-nullable types, though - which can cause warnings since `IsNotNull` is not changing the
      asserted type (as it is not required to break on the first error)
    - To circumvent the "Hey it can be null even though you probably break on the `IsNotNul`-assertion"-warnings, you can use the following pattern:

```csharp
Validator.Check()
    .That(myValue).IsNotNull()
    .That(myValue!).IsNotEmpty();
```

# 4.2.0

- Add overload for `IAssertable<string>.Matches()` accepting a `Regex` directly
- Add new assertion `IAssertable<string>.IsNotOnlyWhitespace`

# 4.1.1

- Fix `IsLessThan` and `IsLessThanOrEqual` extensions to produce the correct error message
    - It falsely claimed "Foo must be greater than 1, but was 2." before - not it will correctly produce "Foo must be less than 1, but was 2."

# 4.1.0

- Add assertion-overloads for all `DateTime` (like `IsAfter` or `IsUtc`) and comparison assertions (`IsLessThan`, `IsGreaterThan`, `IsInRange` and
  others)
    - These assertions will *always* fail for null values, except `IsNotEqualTo` - which will always pass for null values

# 4.0.0

- Name of asserted value is now determined via `CallerArgumentExpression`-Attribute for overloads of `That()` accepting `T` and `Func<T>` (thanks
  Marco for that hint!)
    - When accepting a `Func<T>`, the `() =>`-part is removed from the displayed name
    - The name can still be overriden by passing a name explicitly
- Overload of `That()` accepting an `Expression<Func<T>>` has been removed
- `Validator.Check()` no longer returns a `Validator` directly but an `IValidation` instead
- Extensions to the fluent grammar of `IValidation` have been moved from `Swallow.Validation.Core` to `Swallow.Validation.Assertions`
    - The namespace is still `Swallow.Validation`
    - Affected are overloads of `That`, `When`, `Unless`, `Satisfies` as well as `Then`, `Else` and `ElseThrow`
- `ValidationContainer` has been moved from `Swallow.Validation.Core` to `Swallow.Validation.Assertions`
    - The namespace is still `Swallow.Validation`
- Collection extensions (`IsNotEmpty`, `IsIn`, `IsNotIn`, `HasAll`, `HasAllValid`) have been moved from `Swallow.Validation.Assertions.Collections`
  to `Swallow.Validation.Assertions`
- DateTime extensions (`IsBefore`, `IsAfter`, `IsBetween`, `isUtc`, `IsLocalTime`) have been moved from `Swallow.Validation.Assertions.DateTime`
  to `Swallow.Validation.Assertions`
- Object extensions (`IsType`, `IsAssignableTo`) have been moved from `Swallow.Validation.Assertions.Types` to `Swallow.Validation.Assertions`
- Validatable entity extensions (`IsValid`) have been moved from `Swallow.Validation.Assertions.Validatable` to `Swallow.Validation.Assertions`

If you've only referenced `Swallow.Validation.Core` in your projects, you might need to start referencing `Swallow.Validation` instead - which
includes both Core and `Swallow.Validation.Assertions`. If you're already referencing `Swallow.Validation`, you'll only need to adjust your usings.
And you can finally drop all the `nameof()`s in your `That()`-calls, yay!

# Before 4.0.0

Forever lost to the bottomless abyss that is time...

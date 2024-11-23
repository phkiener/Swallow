# Swallow.Validation

`Swallow.Validation` is a lightweight, fluent-style validation library for C#. But what does that mean? It means minimal dependencies (none, actually)
and great developer experience. If you've seen [Fluent Assertions](https://fluentassertions.com/), you might find some similarities.

## Usage

| Package                                                                                                                                                                                                                                      | Description                                                                                                                                   |
|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------|
| [![Swallow.Validation](https://img.shields.io/nuget/v/Swallow.Validation?style=for-the-badge&logo=nuget&label=Swallow.Validation)](https://www.nuget.org/packages/Swallow.Validation/)                                                       | The "batteries included" version containing the underlying validation engine and a collection of useful assertions for your day-to-day usage. |
| [![Swallow.Validation.Core](https://img.shields.io/nuget/v/Swallow.Validation?style=for-the-badge&logo=nuget&label=Swallow.Validation.Core)](https://www.nuget.org/packages/Swallow.Validatio.Coren/)                                        | The bare validation engine without any assertions at all. Useful when you want to have complete control over the actual validation going on.  |
| [![Swallow.Validation.ServiceCollection](https://img.shields.io/nuget/v/Swallow.Validation?style=for-the-badge&logo=nuget&label=Swallow.Validation.ServiceCollection)](https://www.nuget.org/packages/Swallow.Validation.ServiceCollection/) | Adapter to discover and register asserters in a `ValidationContainer` inside a service collection.                                            |

## Features

`Swallow.Validation` does not require a separate class to contain the validation logic - you can just drop it right into your code without problems.
This allows you to enforce invariants rather than validate the state of entities; instead of constructing a person and checking whether it is correct,
you can make sure that no person created can ever be in an invalid state - without needing to remember calling
`new PersonValidator().Validate(myPerson)`.

The library is also "natively extensible", meaning you can create custom validation rules that perfectly blend in together with the predefined
validation rules and the fluent style. You can create rules matched for your problem domain and use your own ubiquitous language. With the fluent
style of the library, you can formulate validation rules directly in your domain language.

```csharp
Validator.Check()
    .That(Country).IsValidIsoCode()
    .That(DeliveryDate).IsAfter(DateTime.UtcNow)
    .That(Items).IsNotNull().IsNotEmpty()
    .ElseThrow()
```

Additionally, the validation errors are strongly typed. Instead of throwing around strings everywhere, you have meaningful errors containing
properties to describe the problem in an exact manner. You also need not worry about wording your error messages when doing your validation. You can
also easily test validation logic this way; instead of comparing strings, you can just check if the error is of the correct type. You can even
localize the error messages if you'd like; that way, the validation errors can even be given directly to the user.

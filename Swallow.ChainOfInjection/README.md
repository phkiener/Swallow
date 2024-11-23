# Swallow.ChainOfInjection

Behind the quite ominous name is hiding a very generic and abstract way of registering a structure that resembles a Chain of Responsibility to any
DI-container, like [Simple Injector](https://simpleinjector.org/) or
[ServiceCollection](https://docs.microsoft.com/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection?view=dotnet-plat-ext-3.1).

The package in `Swallow.ChainOfInjection` defines the way it is supposed to work - but that is not enough! In order to use this functionality, you
need to provide a specific configurator for the DI-container of your choice. You can find examples of exactly this for SimpleInjector and the
IServiceCollection, if you'd like an example.

The original concept came from this really helpful [Answer on StackOverflow](https://stackoverflow.com/a/55476379), but I've changed quite a lot of it
to make it generic and less "Expression"-y. The general idea, however, remains the same.

## Licensing

`Swallow.ChainOfInjection` is licensed under the MIT license. That means you can do whatever you like with it, as long as you give credit by including
the library's license when distributing your software.

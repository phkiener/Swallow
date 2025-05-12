![Swallow](./assets/swallow-icon-round.svg)

---

[![Swallow.Validation](https://img.shields.io/nuget/v/Swallow.Validation?style=for-the-badge&logo=nuget&label=Swallow.Validation)](./projects/Swallow.Validation/README.md)
&nbsp;
[![Swallow.Refactor](https://img.shields.io/nuget/v/Swallow.Refactor?style=for-the-badge&logo=nuget&label=Swallow.Refactor)](./projects/Swallow.Refactor/README.md)
&nbsp;
[![Swallow.ChainOfInjection](https://img.shields.io/nuget/v/Swallow.ChainOfInjection?style=for-the-badge&logo=nuget&label=Swallow.ChainOfInjection)](./projects/Swallow.ChainOfInjection/README.md)
&nbsp;
[![Swallow.TaskRunner](https://img.shields.io/nuget/v/Swallow.TaskRunner?style=for-the-badge&logo=nuget&label=Swallow.TaskRunner)](./projects/Swallow.TaskRunner/README.md)
&nbsp;
[![Swallow.Flux](https://img.shields.io/nuget/v/Swallow.Flux?style=for-the-badge&logo=nuget&label=Swallow.Flux)](./projects/Swallow.Flux/README.md)
&nbsp;
![MIT license](https://img.shields.io/badge/license-mit-brightgreen?style=for-the-badge)

---

# Swallow

A collection of low-dependency packages to be used in any scenario.

They don't strive to be the best-in-class or the most performant. Instead, they all aim to be simple and agnostic of any framework or context; just
plug these libraries into your code and work with them however you want. They carry as few dependencies as possible, leaving only a tiny footprint
behind.

There's no common theme to these - it's simply a big potluck of stuff that I've either used multiple times or simply wanted to build for the fun of
it.

By the way, **Swallow** refers to the [bird](https://en.wikipedia.org/wiki/Swallow). That's also what the logo is trying to depict.

## Projects

* [Swallow.Validation](./projects/Swallow.Validation/README.md) - fluent and extendable validations for your invariants
* [Swallow.Refactor](./projects/Swallow.Refactor/README.md) - automatic refactoring goes BRRR!
* [Swallow.ChainOfInjection](./projects/Swallow.ChainOfInjection/README.md) - declarative registration of decorators for `ServiceCollection` and `SimpleInjector`
* [Swallow.TaskRunner](./projects/Swallow.TaskRunner/README.md) - miss `npm run $MYTASK` or similar tools? How'd you like `dotnet task $MYTASK`?
* [Swallow.Flux](./projects/Swallow.Flux/README.md) - no-nonsense Flux pattern, completely framework-agnostic
* Work in progress; unstable, unreleased and unfinished
  * [Swallow.Localization](./projects/Swallow.Localization/README.md) - addition tooling built on `IStringLocalizer`
  * [Swallow.Console](./projects/Swallow.Console/README.md) - the convenience of ASP.NET - inside your terminal
  * [Swallow.Charts](./projects/Swallow.Charts/README.md) - statically rendered charts for Blazor applications
  * [Swallow.Build](./projects/Swallow.Build/README.md) - miscellaneous goodness to smoothen your builds using only MSBuild
* Tooling for the monorepo
  * [Swallow.Manager](./tooling/Swallow.Manager) - create and publish projects

## Why should I use these?

Eh, you probably don't want to. If you want to rely on bulletproof, battle-tested, production-grade and high-performance libraries, you'll not find
them in here. But: They're all MIT licensed and will not do a "thank you for using my stuff, now pay up" years down the line. It's a hobby, not
something to extract value from.

But if you want a library that does what you need, stays out of your way and doesn't add a swath of transitive dependencies - why not give it a try?

## License

Each project has its own license, but they all are and will always be released under the MIT license. Which means you're free to use all of these
projects however you see fit, as long as you include the original license when redistributing. What does "redistributing" include? I don't know, just
don't claim that you invented the stuff and you're good. I'm a dev, not a lawyer.

## Contributing

There's no process or whatever. Just create an issue or submit your pull request and I'll take a look... eventually.
Seriously, there's no guarantees. You might get a response right away, you might have to wait a few months. Don't expect anything.

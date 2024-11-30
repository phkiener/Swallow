# Swallow

A collection of low-dependency packages joined together in a monorepo for
my personal convenience. In actual usage, they are completely independent.
Mostly infrastructure (MSBuild setup, pipelines, assets, ...) are shared
between the different projects.


## Projects

|                                                          Project | Description                                                                                  | Status                                                                                                                                                                                                         |
|-----------------------------------------------------------------:|:---------------------------------------------------------------------------------------------|:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
|             [Swallow.Validation](./Swallow.Validation/README.md) | Object validation and assertions, not for testing but for production code.                   | [![Swallow.Validation](https://img.shields.io/nuget/v/Swallow.Validation?style=for-the-badge&logo=nuget&label=Swallow.Validation)](https://www.nuget.org/packages/Swallow.Validation/)                         |
|                 [Swallow.Refactor](./Swallow.Refactor/README.md) | Automatic refactoring and other Roslyn-shenanigans                                           | [![Swallow.Refactor](https://img.shields.io/nuget/v/Swallow.Refactor?style=for-the-badge&logo=nuget&label=Swallow.Refactor)](https://www.nuget.org/packages/Swallow.Refactor/)                                 |
| [Swallow.ChainOfInjection](./Swallow.ChainOfInjection/README.md) | DI registration for chains of decorators                                                     | [![Swallow.ChainOfInjection](https://img.shields.io/nuget/v/Swallow.ChainOfInjection?style=for-the-badge&logo=nuget&label=Swallow.ChainOfInjection)](https://www.nuget.org/packages/Swallow.ChainOfInjection/) |
|             [Swallow.TaskRunner](./Swallow.TaskRunner/README.md) | Repository-based shortcuts for common CLI tasks, much like `deno task` or similar toolchains | [![Swallow.TaskRunner](https://img.shields.io/nuget/v/Swallow.TaskRunner?style=for-the-badge&logo=nuget&label=Swallow.TaskRunner)](https://www.nuget.org/packages/Swallow.TaskRunner/)                         |
|         [Swallow.Localization](./Swallow.Localization/README.md) | Localization management on the console... and maybe some more cool stuff                     | Still experimenting                                                                                                                                                                                            |
|                     [Swallow.Charts](./Swallow.Charts/README.md) | Statically rendered charts as Blazor components                                              | Still experimenting                                                                                                                                                                                            |

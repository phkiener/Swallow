# Swallow.Refactor

> Refactor goes BRRR!

A tool that was originally designed as a way to enable reliably refactor a big code-base automatically, it
has since turned into a generic interface to the Roslyn API to interact with any C# code, be it just
reading and analyzing or even modifying.

What's BRRR short for, you ask? **B**oundless **R**oslyn **R**efactoring **R**unner.

## BRRRusage

```sh
brrr refactor -s|--solution <solution> code [--filter-name <name>] [--filter-content <content>] <rewriters>
```

When calling `refactor`, instead of passing all rewriters via CLI you can instead pass the path to a JSON file.
This file must match the following scheme:

```json5
{
  "filter": {
    "name": "<some regex>", // A regex that filenames should match, optional
    "content": "<some text>" // A text that files should contain, optional
  },
  "rewriters": [
    { "name": "<some rewriter>" }, // A rewriter without any parameters
    { "name": "<some rewriter>", "parameters": [ "parameter", "parameter" ] } // A rewriter with parameters
  ]
}
```

To find out what rewriters are available, you can use `brrr rewriter list` and `brrr rewriter describe <rewriter>`.

But it doesn't stop there - there's a lot of things you can do. Using `brrr asyncify`, for example, allows you to
turn a chain of calls into async calls automatically, fixing any `GetAwaiter().GetResult()`-calls along the way.

To see all commands that you can use, run
```sh
brrr --help
```

## BRRRinstallation

### Building and installing yourself

To install the tool (and thus make it available globally), you can use the following command:
```sh
dotnet pack -c Release -o packages/
dotnet tool install --global --no-cache --add-source packages/ Swallow.Refactor
```

This will make `brrr` be a globally executable tool. To uninstall it again, you can execute:
```sh
dotnet tool uninstall --global Swallow.Refactor
```

#### Note for users of _just_

If you've got [just](https://github.com/casey/just) installed, you can use `just install`, `just uninstall` or `just reinstall`!
Take a look at the [justfile](justfile), there're some neat things for you.

### Installing via NuGet

The tool is published on [NuGet](https://www.nuget.org/packages/Swallow.Refactor) as well. To install it from there, you can use the following command:
```sh
dotnet tool install --global Swallow.Refactor
```

## BRRRlugins

If you want to use your own rewriters, commands, symbol filters, you name it - you can pass your own assemblies to the execution.
They will get picked up and the relevant classes are available in all the use-cases as if they were embedded in the program!

Invoking the tool using the `-p` or `--plugin` option passing a list of semicolon-separated filepaths will load each assembly.
```sh
brrr -p|--plugin SomeAssembly.dll;SomeOtherAssembly.dll list
```

Using this, you can extend the basic behaviour in many ways, like defining additional commands to be executed or  registering
additional `IDocumentRewriter`s or `ITargetedRewriter`s.

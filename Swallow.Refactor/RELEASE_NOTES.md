# Release Notes

## 3.1.0

Upgrade all dependencies, most notably Roslyn (`Microsoft.CodeAnalysis.*`) to 4.9.2.

## 3.0.0

Upgrade to .NET 8 and add another kind of rewriter: the `ITargetedRewriter`.

### Just the big-picture stuff, please

A new type of rewriter, the `ITargetedRewriter`, has been introduced that will act on `ISymbol`s instead of `Document`s. To start, `RenameSymbol`
is provided as the only rewriter of this type - it will... well... rename any symbol in definition and usages.

To make a clear cut, the existing `IRewriter` has been renamed to `IDocumentRewriter` and related types (like the `IRewriterFactory`) and properties
have been renamed as well.

On the execution-side, `BaseCommand` is now called `ProgressCommand` and needs to implement `RunAsync` instead of `ExecuteAsync`.
The new `BaseCommand` provides only the `IFeatureCollection`-related stuff and does not start a `AnsiConsole.Progress`, allowing to to control
the output a bit more if you desire. All `RunAsync` or `ProcessAsync` operations are defined for `ProgressCommand`, so chances are you want to
target that class instead of staying on `BaseCommand`.

### General

- Update all dependencies and move to .NET 8

### Abstractions

- Add `ITargetedRewriter`, accepting a `SolutionEditor` and a target `ISymbol`
- Add `ITargetedRewriterFactory` and add it as property to `IRegistry`
- Rename `IRewriter` to `IDocumentRewriter` and `IRewriterFactory` to `IDocumentRewriterFactory`
  - Related properties have been renamed to match the new names as well

### Core

- Add `RenameSymbol` as first targeted rewriter
- Add `Rename` node modification (as extension to `SyntaxNode`)

### Execution

- `RegistrationExtensions.Register` now has an overload accepting two type parameters
  - This is useful when registering different branches for the command, e.g. `refactor code` and `refactor symbol`, which share a base settings class
- Split `BaseCommand` into `BaseCommand` and `ProgressCommand`
  - `BaseCommand` is jus a wrapper around `AsyncCommand` enhanced with the `IFeatureCollection`, all the different `RunAsync` or `ProcessAsync` methods
    are _not_ available for it.
  - `ProgressCommand` behaves like the previous `BaseCommand` - instead of `ExecuteAsync`, you have to implement `RunAsync`, however.

### Testing

- Add `CommandTest.Registry` and include this factory as `IRegistryFeature` in the running command
  - You can use `IncludeRewriter`, `IncludeTargetedRewriter` and `IncludeSymbolFilter` to include certain rewriters to your executed test
  - `FixedRegistry` and `IFixedRegistryFeature` have been added to enable this use-case
- Rename `RewriterTest` to `DocumentRewriterTest`
- Add `TargetedRewriterTest` which is just like the `DocumentRewriterTest` - but for `ITargetedRewriter`s

### Tool

- Change syntax for `refactor` command as follows:
  - The current behaviour (refactor code on a file-basis) is now invoked using `refactor code [rewriters]`
  - If you wish to use targeted rewriters, you can now use `refactor symbol [rewriters]`
  - Please see the help text for both commands to know more about where to put the options for both commands!

---

## 2.1.0

### Tool

- Remove the notice regarding asyncify being experimental
- Ensure asyncify handles already existing async overloads of methods (thanks Pipo!)

---

## 2.0.1

### Tool

- Fix missing log when opening the workspace via workspace feature

---

## 2.0.0

A big release with many big changes.

### Just the big-picture stuff, please

**If you are a user**: The most notable change is the new `asyncify` command that will turn your synchronous methods async, working its way up to all
the callers as well. Where possible, calls will be `await`ed. `CancellationToken`s will neither be added nor used. It should work without problems,
but as code looks different in every project, the command might hit some edge-cases and... fall apart. Use a clean state, you might need
to rollback the changes!

**If you are a developer**: The most notable change is how commands are implemented. First and foremost: You can test them using `CommandTest`!
All the output (via `BaseCommand.Console` or `BaseCommand.Logger`) is routed to the console out, visible after the test has finished. You can assert
it via `CommandTest.TestConsole.Output` or `CommandTest.TestConsole.Lines`.

In addition to that, *infrastructure* concerns such as logging are setup
as `IFeatureCollection` - the existing features are exposed as properties on `BaseCommand`. In the future, you might be able to define your own
*features* and have them loaded at runtime, but that's something for 3.0.0 I reckon. In the meantime, enjoy the following change:
If your settings implement `IHasSolution`, you no longer need to open the solution yourself - it will be loaded automatically before the command
is executed.

### Tool

- Added `Asyncify` command to automatically transform a method call into an async call - moving upwards to callers
- `--version` will now output the correct version
- Only english localizations are included for *Roslyn*, shrinking the package by ~2MB

### Core

- Most refactorings no longer replace the node but instead generate a new one based on the source node
  - `AsyncToSync` no longer works on a `DocumentEditor` but on an `InvocationExpressionSyntax` instead
  - `AwaitCalls` is replaced by a simpler `AwaitCall` that handles a single invocation
  - `AddUsing` on a `DocumentEditor` has been removed
- Add `SyntaxEditorExtensions.RecordChange<T, TOut>` to easily use the generator-syntax of a `SyntaxEditor`
  - A similar extension `SyntaxEditorExtensions.RecordChanges<T, TOut>` exists for lists
- `FindSyntaxNodes` has been removed
- Removed old `Asyncifier` and `CallGraphBuilder` and related classes

### Execution

- Added `IFeatureCollection` with the starting features:
  - `IConsoleFeature` (providing an `IAnsiConsole`)
  - `ILoggerFeature` (providing an `ILogger`)
  - `IRegistryFeature` (building and providing an `IRegistry`)
  - `IWorkspaceFeature` (finding, loading and providing a `Workspace`)
- `IHasSolution` and `IHasLogger` have moved to `Swallow.Refactor.Execution.Settings`
- `IRegisterableCommand` has moved to `Swallow.Refactor.Execution.Registration`
- `SolutionPathOrDefault`-extension method has been removed
  - The solution will automatically be loaded when the settings implement `IHasSolution` - you can just use `BaseCommand.Workspace` directly

### Testing

- `RoslynTest.AreEqual` has moved to `SyntaxAssert.AreEqual` in namespace `Swallow.Refactor.Testing.Assertion`
- Add `SyntaxParser.ParseAs` helper to parse strings into `SyntaxNode`s
- Add `CommandTest` base class for testing commands
- Remove `RefactoringTest` base class

---

## 1.0.1

### Commands
- `references`: Fix immediate references (from the given project) not being included in the output

---

## 1.0.0

Initial Release containing the following commands and rewriters:

### Commands

- `rewriter list`: List all available *rewriters*
- `rewriter describe`: Describe a *rewriter* in greater detail
- `symbol-filter list`: List all available *symbol filters*
- `unused`: Find unused symbols in a project
- `references`: List direct and transitive project references of a project
- `refactor`: Modify code matching certain criteria using *rewriter*s

### Rewriters

- `AddUsing`: Add a using directive to a file
- `ChangeInjectedMember`: Modify the type and name of a member that is injected via constructor
- `OptimizeUsings`: Sort usings and remove those that are not required
- `RenameIdentifier`: Rename all occurrences of a certain identifier
- `ReplaceCode`: Replace code with another expression
- `ReplaceFieldMemberAccess`: Replace all using of one member with a different expression

In addition to the tool, the following packages are available:

- `Swallow.Refactor.Abstractions`: Interfaces neded to provide *plugins* to `Swallow.Refactor`, embeddable via CLI using `-p <path-to-dll>`
- `Swallow.Refactor.Core`: Modifications, read helpers and other already-built functionality to use when writing *plugins*
- `Swallow.Refactor.Execution`: Base classes used to define additional commands in *plugins*, invokable as usual via CLI
- `Swallow.Refactor.Testing`: Unit testing helpers to ease testing of rewriters, commands and other helpers

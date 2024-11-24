# Swallow.TaskRunner

A "task runner" to run common operations via commandline. The alternative to launch profiles when solely working on the CLI. Because why should only
other toolchains get nice stuff?

This tool is greatly influence by the way `dotnet tool` works for repository-local tools to feel familiar.

### Why not use launch profiles?
Because you're not always working in an IDE. Doing these via CLI feels.. clunky.

### Why not use justfile/makefile/some other tool?
Whatever floats your boat! This is just an alternative to feel "native" for .NET.

### Why not use MSBuild targets?
MSBuild is to this what a anti-material missle launcher is to a slingshot. Yes, you can launch a stone with both. But are you really gonna do that?

## Getting started

Install the tool and initialize a new manifest by running `dotnet task new-manifest`. This will create a file `.config/dotnet-tasks.json` relative
to the _current directory_. When invoking `dotnet task`, it will look for this manifest going upwards, starting from the current directory.

### The task manifest

```json
{
  "Version": 1,
  "shell-command": "echo 'This is some text I can print out'",
  "shell-sequence": [
    "echo 'Do this first'",
    "echo 'Do this second'",
    "exit 255",
    "echo 'Never do this!"
  ]
}
```

## Roadmap

- [x] Running plain shell commands
- [ ] Running "abstract" commands like "copy", "remove" that work across shells
- [x] Sequential tasks, i.e. many commands in order (with proper cancellation!)
- [ ] Nested names for tasks ("my-stuff other-stuff foo")
- [ ] Parameters for tasks
- [ ] Specifying default tasks
- [x] Adding/removing tasks via CLI

# Release Notes

## 0.0.1

First preview release of this little experiment.

Install it as a local tool in your repository and run `dotnet task new-manifest` to create a _task manifest_.
Then use `dotnet task add <NAME> <COMMAND>` to add tasks (or edit the manifest directly). Calling `dotnet task <NAME>` then invokes that task.

It's not that stable right now, so don't rely on it too much. Here goes dogfooding, I guess.

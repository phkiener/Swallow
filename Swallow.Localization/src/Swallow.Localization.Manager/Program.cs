using Swallow.Localization.Manager.Commands;

if (args is ["-v", ..] or ["--version", ..])
{
    await PrintVersion.RunAsync();
    return 0;
}

if (args is ["-h", ..] or ["--help", ..])
{
    await PrintHelp.RunAsync();
    return 0;
}

if (args is ["list", ..var rest])
{
    await ListResources.RunAsync(rest);
    return 0;
}

await PrintHelp.RunAsync();
return 1;

using Swallow.SiteGen;

if (args is ["serve", var url])
{
    await Serve.RunAsync(url);
    return 0;
}

if (args is ["build", var targetPath])
{
    await Build.RunAsync(targetPath);
    return 0;
}

Console.Error.WriteLine("Unknown command.");
return 1;

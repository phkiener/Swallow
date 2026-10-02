using Swallow.SiteGen;

var task = args switch
{
    ["serve", var url] => Generator.ServeSiteAsync(url),
    ["build", var targetPath] => Generator.BuildSiteAsync(targetPath),
    _ => Task.FromException(new InvalidOperationException("Unknown command."))
};

try
{
    await task;
    return 0;
}
catch (Exception e)
{
    Console.Error.WriteLine(e.Message);
    return 1;
}

namespace Swallow.Manager.Commands;

public static class Help
{
    public static void Run()
    {
        Console.WriteLine("Swallow Monorepo Manager");
        Console.WriteLine();
        Console.WriteLine("Usage: ");
        Console.WriteLine("  swallow generate               - (Re-)generate all solution files based on the repository structure");
        Console.WriteLine("  swallow add $NAME              - Add a new package called Swallow.$NAME and regenerate the solution(s)");
        Console.WriteLine("  swallow publish $NAME $VERSION - Publish Swallow.$NAME to a set of NuGet packages, tagged with the given version");
        Console.WriteLine();
    }
}

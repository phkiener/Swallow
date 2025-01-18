namespace Swallow.Console.Arguments;

[TestFixture]
public sealed class ArgumentParserTest
{
    [Test]
    public void CanBeInvoked()
    {
        Assert.Throws<NotImplementedException>(() => ArgParse.Parse<int>([""]));
    }
}

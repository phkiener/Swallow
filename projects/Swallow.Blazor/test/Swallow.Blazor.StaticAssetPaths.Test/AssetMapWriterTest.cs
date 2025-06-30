using System.Diagnostics.CodeAnalysis;

namespace Swallow.Blazor.StaticAssetPaths;

[TestFixture]
public sealed class AssetMapWriterTest
{
    private StringWriter writer = null!;

    [SetUp]
    public void SetUp()
    {
        writer = new StringWriter();
    }

    [TearDown]
    public void TearDown()
    {
        writer.Dispose();
        writer = null!;
    }

    [Test]
    public void EmptyAssetMapIsWritten()
    {
        var assetMap = new AssetMap([]);
        AssetMapWriter.WriteTo(writer, assetMap);

        AssertGeneratedText("""
            public static class AssetPaths
            {
            }
            """);
    }

    private void AssertGeneratedText([StringSyntax("C#")] string expected)
    {
        // We don't really care about any surrounding whitespace.
        Assert.That(writer.ToString().Trim(), Is.EqualTo(expected));
    }
}

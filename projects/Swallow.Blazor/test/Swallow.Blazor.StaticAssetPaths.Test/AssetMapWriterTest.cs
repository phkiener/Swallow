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

    [Test]
    public void WwwrootAssetsAreWritten()
    {
        var assetMap = new AssetMap([
            DefinedAsset.For("wwwroot/global.css"),
            DefinedAsset.For("wwwroot/app.js"),
            DefinedAsset.For("wwwroot/assets/logo/logo.png")
        ]);

        AssetMapWriter.WriteTo(writer, assetMap);

        AssertGeneratedText("""
                            public static class AssetPaths
                            {
                                public static class Wwwroot
                                {
                                    /// <summary>
                                    /// Points to <c>wwwroot/app.js</c>
                                    /// </summary>
                                    public static readonly string AppJs = "app.js";

                                    /// <summary>
                                    /// Points to <c>wwwroot/global.css</c>
                                    /// </summary>
                                    public static readonly string GlobalCss = "global.css";

                                    public static class Assets
                                    {
                                        public static class Logo
                                        {
                                            /// <summary>
                                            /// Points to <c>wwwroot/assets/logo/logo.png</c>
                                            /// </summary>
                                            public static readonly string LogoPng = "assets/logo/logo.png";
                                        }
                                    }
                                }
                            }
                            """);
    }

    [Test]
    public void ModuleScriptsAreWritten()
    {
        var assetMap = new AssetMap([
            DefinedAsset.For("App.razor.js"),
            DefinedAsset.For("Page.cshtml.js"),
            DefinedAsset.For("Deeply/Nested/Component.razor.js")
        ]);

        AssetMapWriter.WriteTo(writer, assetMap);

        AssertGeneratedText("""
                            public static class AssetPaths
                            {
                                /// <summary>
                                /// Points to <c>App.razor.js</c>
                                /// </summary>
                                public static readonly string AppRazorJs = "App.razor.js";

                                /// <summary>
                                /// Points to <c>Page.cshtml.js</c>
                                /// </summary>
                                public static readonly string PageCshtmlJs = "Page.cshtml.js";

                                public static class Deeply
                                {
                                    public static class Nested
                                    {
                                        /// <summary>
                                        /// Points to <c>Deeply/Nested/Component.razor.js</c>
                                        /// </summary>
                                        public static readonly string ComponentRazorJs = "Deeply/Nested/Component.razor.js";
                                    }
                                }
                            }
                            """);
    }

    [Test]
    public void ComponentsAndRazorPagesAreIgnored()
    {
        var assetMap = new AssetMap([
            DefinedAsset.For("App.razor"),
            DefinedAsset.For("Page.cshtml")
        ]);

        AssetMapWriter.WriteTo(writer, assetMap);

        AssertGeneratedText("""
                            public static class AssetPaths
                            {
                            }
                            """);
    }

    [Test]
    public void PathsAreProperIdentifiers()
    {
        var assetMap = new AssetMap([
            DefinedAsset.For("wwwroot/snake_case.css"),
            DefinedAsset.For("wwwroot/kebap_case.js"),
            DefinedAsset.For("wwwroot/PascalCase.png"),
            DefinedAsset.For("wwwroot/camelCase.svg")
        ]);

        AssetMapWriter.WriteTo(writer, assetMap);

        AssertGeneratedText("""
                            public static class AssetPaths
                            {
                                public static class Wwwroot
                                {
                                    /// <summary>
                                    /// Points to <c>wwwroot/camelCase.svg</c>
                                    /// </summary>
                                    public static readonly string CamelCaseSvg = "camelCase.svg";

                                    /// <summary>
                                    /// Points to <c>wwwroot/kebap_case.js</c>
                                    /// </summary>
                                    public static readonly string KebapCaseJs = "kebap_case.js";

                                    /// <summary>
                                    /// Points to <c>wwwroot/PascalCase.png</c>
                                    /// </summary>
                                    public static readonly string PascalCasePng = "PascalCase.png";

                                    /// <summary>
                                    /// Points to <c>wwwroot/snake_case.css</c>
                                    /// </summary>
                                    public static readonly string SnakeCaseCss = "snake_case.css";
                                }
                            }
                            """);
    }

    [Test]
    public void BackslashesWorkToo()
    {
        var assetMap = new AssetMap([DefinedAsset.For("Windows\\Is\\Special.razor.js")]);
        AssetMapWriter.WriteTo(writer, assetMap);

        AssertGeneratedText("""
                            public static class AssetPaths
                            {
                                public static class Windows
                                {
                                    public static class Is
                                    {
                                        /// <summary>
                                        /// Points to <c>Windows/Is/Special.razor.js</c>
                                        /// </summary>
                                        public static readonly string SpecialRazorJs = "Windows/Is/Special.razor.js";
                                    }
                                }
                            }
                            """);
    }

    private void AssertGeneratedText([StringSyntax("C#")] string expected)
    {
        // We don't really care about any surrounding whitespace.
        var actualText = writer.ToString().Replace(" ", "").ReplaceLineEndings("");
        var expectedText = expected.Replace(" ", "").ReplaceLineEndings("");
        Assert.That(actualText, Is.EqualTo(expectedText));
    }
}

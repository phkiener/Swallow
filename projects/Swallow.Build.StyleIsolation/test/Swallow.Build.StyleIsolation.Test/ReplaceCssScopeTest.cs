namespace Swallow.Build.StyleIsolation;

[TestFixture]
public sealed class ReplaceCssScopeTest
{
    private ReplaceCssScope task = null!;

    [SetUp]
    public void SetUp()
    {
        task = new ReplaceCssScope { BuildEngine = new FakeBuildEngine() };
    }

    [Test]
    public void Works()
    {
        task.Items = [new InheritStyleItem(Path: "Derived.razor", Inherit: "Base.razor")];
        task.Components = [new RazorComponentItem(Path: "Base.razor", CssScope: "b-aaaaaaaaaa"), new RazorComponentItem(Path: "Derived.razor", CssScope: "b-bbbbbbbbbb")];
        task.Styles = [new ScopedCssItem(Path: "Base.razor.css", CssScope: "b-aaaaaaaaaa"), new ScopedCssItem("Derived.razor.css", "b-bbbbbbbbbb")];

        task.Execute();

        var component = task.AdjustedComponents.Single();
        Assert.That(component.ItemSpec, Is.EqualTo("Derived.razor"));
        Assert.That(component.GetMetadata("CssScope"), Is.EqualTo("b-aaaaaaaaaa"));

        var style = task.AdjustedStyles.Single();
        Assert.That(style.ItemSpec, Is.EqualTo("Derived.razor.css"));
        Assert.That(style.GetMetadata("CssScope"), Is.EqualTo("b-aaaaaaaaaa"));
    }
}

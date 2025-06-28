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
        task.RazorComponents = [new RazorComponentItem(Path: "Base.razor", CssScope: "b-aaaaaaaaaa"), new RazorComponentItem(Path: "Derived.razor", CssScope: null)];
        task.Styles = [new RazorComponentItem(Path: "Base.razor.css", CssScope: "b-aaaaaaaaaa")];

        task.Execute();

        Assert.That(task.AdjustedStyles, Is.Empty);
        Assert.That(task.AdjustedComponents, Is.Not.Empty);

        var item = task.AdjustedComponents.Single();
        Assert.That(item.ItemSpec, Is.EqualTo("Derived.razor"));
        Assert.That(item.GetMetadata("CssScope"), Is.EqualTo("b-aaaaaaaaaa"));
    }
}

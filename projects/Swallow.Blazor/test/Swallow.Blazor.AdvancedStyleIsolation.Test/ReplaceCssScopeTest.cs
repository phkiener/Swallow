using Swallow.Blazor.AdvancedStyleIsolation.Framework;

namespace Swallow.Blazor.AdvancedStyleIsolation;

[TestFixture]
public sealed class ReplaceCssScopeTest
{
    private FakeBuildEngine buildEngine = null!;
    private ReplaceCssScope task = null!;

    [SetUp]
    public void SetUp()
    {
        buildEngine = new FakeBuildEngine();
        task = new ReplaceCssScope { BuildEngine = buildEngine };
    }

    [Test]
    public void DoesNothing_WhenInheritedComponentDoesNotExist()
    {
        task.Items = [new InheritStyleItem(Path: "Component.razor", From: "Fake.razor")];
        task.Components = [new RazorComponentItem(Path: "Component.razor", CssScope: "b-aaaaaaaaaa")];
        task.Styles = [new ScopedCssItem(Path: "Component.razor.css", CssScope: "b-aaaaaaaaaa")];

        task.Execute();

        Assert.That(task.AdjustedComponents, Is.Empty);
        Assert.That(task.AdjustedStyles, Is.Empty);
        Assert.That(buildEngine.Logs, Is.Not.Empty);
        Assert.That(buildEngine.Logs.Single(), Does.Contain("WARN").And.Contain("Component not found"));
    }

    [Test]
    public void DoesNothing_WhenInheritedComponentDoesNotHaveStyles()
    {
        task.Items = [new InheritStyleItem(Path: "Component.razor", From: "Fake.razor")];
        task.Components = [new RazorComponentItem(Path: "Component.razor", CssScope: "b-aaaaaaaaaa"), new RazorComponentItem("Fake.razor", CssScope: null)];
        task.Styles = [new ScopedCssItem(Path: "Component.razor.css", CssScope: "b-aaaaaaaaaa")];

        task.Execute();

        Assert.That(task.AdjustedComponents, Is.Empty);
        Assert.That(task.AdjustedStyles, Is.Empty);
        Assert.That(buildEngine.Logs, Is.Not.Empty);
        Assert.That(buildEngine.Logs.Single(), Does.Contain("WARN").And.Contain("Component does not have a CSS scope"));
    }

    [Test]
    public void Fails_WhenInheritBuildsACircle()
    {
        task.Items = [new InheritStyleItem(Path: "A.razor", From: "B.razor"), new InheritStyleItem(Path: "B.razor", From: "A.razor")];
        task.Components = [new RazorComponentItem(Path: "A.razor", CssScope: "b-aaaaaaaaaa"), new RazorComponentItem("B.razor", CssScope: "b-0000000000")];
        task.Styles = [new ScopedCssItem(Path: "A.razor.css", CssScope: "b-aaaaaaaaaa"), new ScopedCssItem("B.razor.css", CssScope: "b-0000000000")];

        var result = task.Execute();

        Assert.That(result, Is.False);
        Assert.That(task.AdjustedComponents, Is.Null);
        Assert.That(task.AdjustedStyles, Is.Null);
        Assert.That(buildEngine.Logs, Is.Not.Empty);
        Assert.That(buildEngine.Logs.Single(), Does.Contain("ERR").And.Contain("Loops detected"));
    }

    [Test]
    public void SetsScopeOfComponent_ToScopeOfInheritedComponent()
    {

        task.Items = [new InheritStyleItem(Path: "Derived.razor", From: "Base.razor")];
        task.Components = [new RazorComponentItem(Path: "Base.razor", CssScope: "b-aaaaaaaaaa"), new RazorComponentItem(Path: "Derived.razor", CssScope: null)];
        task.Styles = [new ScopedCssItem(Path: "Base.razor.css", CssScope: "b-aaaaaaaaaa")];

        task.Execute();

        Assert.That(task.AdjustedComponents, Has.One.Items);
        Assert.That(task.AdjustedComponents.Single().ItemSpec, Is.EqualTo("Derived.razor"));
        Assert.That(task.AdjustedComponents.Single().GetMetadata("CssScope"), Is.EqualTo("b-aaaaaaaaaa"));

        Assert.That(task.AdjustedStyles, Is.Empty);
    }

    [Test]
    public void SetsScopeOfStysheetForComponent_ToScopeOfInheritedComponent()
    {
        task.Items = [new InheritStyleItem(Path: "Derived.razor", From: "Base.razor")];
        task.Components = [new RazorComponentItem(Path: "Base.razor", CssScope: "b-aaaaaaaaaa"), new RazorComponentItem(Path: "Derived.razor", CssScope: "b-0000000000")];
        task.Styles = [new ScopedCssItem(Path: "Base.razor.css", CssScope: "b-aaaaaaaaaa"), new ScopedCssItem(Path: "Derived.razor.css", CssScope: "b-0000000000")];

        task.Execute();

        Assert.That(task.AdjustedComponents, Has.One.Items);
        Assert.That(task.AdjustedComponents.Single().ItemSpec, Is.EqualTo("Derived.razor"));
        Assert.That(task.AdjustedComponents.Single().GetMetadata("CssScope"), Is.EqualTo("b-aaaaaaaaaa"));

        Assert.That(task.AdjustedStyles, Has.One.Items);
        Assert.That(task.AdjustedStyles.Single().ItemSpec, Is.EqualTo("Derived.razor.css"));
        Assert.That(task.AdjustedStyles.Single().GetMetadata("CssScope"), Is.EqualTo("b-aaaaaaaaaa"));
    }
}

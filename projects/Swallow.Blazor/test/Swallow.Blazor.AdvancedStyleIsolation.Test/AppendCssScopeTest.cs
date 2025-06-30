using Swallow.Blazor.AdvancedStyleIsolation.Framework;

namespace Swallow.Blazor.AdvancedStyleIsolation;

[TestFixture]
public sealed class AppendCssScopeTest
{
    private FakeBuildEngine buildEngine = null!;
    private AppendCssScope task = null!;

    [SetUp]
    public void SetUp()
    {
        buildEngine = new FakeBuildEngine();
        task = new AppendCssScope { BuildEngine = buildEngine };
    }

    [Test]
    public void DoesNothing_WhenTargetComponentDoesNotExist()
    {
        task.Items = [new AppendStyleItem(Path: "Component.razor", From: "Fake.razor")];
        task.Components = [new RazorComponentItem(Path: "Component.razor", CssScope: "b-aaaaaaaaaa")];

        task.Execute();

        Assert.That(task.AdjustedComponents, Is.Empty);
        Assert.That(buildEngine.Logs, Is.Not.Empty);
        Assert.That(buildEngine.Logs.Single(), Does.Contain("WARN").And.Contain("Component not found"));
    }

    [Test]
    public void DoesNothing_WhenInheritedComponentDoesNotHaveStyles()
    {
        task.Items = [new AppendStyleItem(Path: "Component.razor", From: "Fake.razor")];
        task.Components = [new RazorComponentItem(Path: "Component.razor", CssScope: "b-aaaaaaaaaa"), new RazorComponentItem("Fake.razor", CssScope: null)];

        task.Execute();

        Assert.That(task.AdjustedComponents, Is.Empty);
        Assert.That(buildEngine.Logs, Is.Not.Empty);
        Assert.That(buildEngine.Logs.Single(), Does.Contain("WARN").And.Contain("Component does not have a CSS scope"));
    }

    [Test]
    public void Fails_WhenInheritBuildsACircle()
    {
        task.Items = [new AppendStyleItem(Path: "A.razor", From: "B.razor"), new AppendStyleItem(Path: "B.razor", From: "A.razor")];
        task.Components = [new RazorComponentItem(Path: "A.razor", CssScope: "b-aaaaaaaaaa"), new RazorComponentItem("B.razor", CssScope: "b-0000000000")];

        var result = task.Execute();

        Assert.That(result, Is.False);
        Assert.That(task.AdjustedComponents, Is.Empty);
        Assert.That(buildEngine.Logs, Is.Not.Empty);
        Assert.That(buildEngine.Logs.Single(), Does.Contain("ERR").And.Contain("Loops detected"));
    }

    [Test]
    public void AddsScopeOfOtherComponent_ToTargetedComponent()
    {

        task.Items = [new AppendStyleItem(Path: "Derived.razor", From: "Base.razor")];
        task.Components = [new RazorComponentItem(Path: "Base.razor", CssScope: "b-aaaaaaaaaa"), new RazorComponentItem(Path: "Derived.razor", CssScope: null)];

        task.Execute();

        Assert.That(task.AdjustedComponents, Has.One.Items);
        Assert.That(task.AdjustedComponents.Single().ItemSpec, Is.EqualTo("Derived.razor"));
        Assert.That(task.AdjustedComponents.Single().GetMetadata("CssScope"), Is.EqualTo("b-aaaaaaaaaa"));
    }

    [Test]
    public void AddsScopeOfOtherComponent_ToTargetedUnstyledComponent()
    {

        task.Items = [new AppendStyleItem(Path: "Derived.razor", From: "Base.razor")];
        task.Components = [new RazorComponentItem(Path: "Base.razor", CssScope: "b-aaaaaaaaaa"), new RazorComponentItem(Path: "Derived.razor", CssScope: "b-0000000000")];

        task.Execute();

        Assert.That(task.AdjustedComponents, Has.One.Items);
        Assert.That(task.AdjustedComponents.Single().ItemSpec, Is.EqualTo("Derived.razor"));
        Assert.That(task.AdjustedComponents.Single().GetMetadata("CssScope"), Is.EqualTo("b-0000000000 b-aaaaaaaaaa"));
    }
}

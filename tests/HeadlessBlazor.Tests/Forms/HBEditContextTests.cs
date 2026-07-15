namespace HeadlessBlazor.Tests.Forms;

/// <summary>
/// Layer 1 (pure logic) tests for <see cref="HBEditContext{TModel}"/>: lazy creation of the
/// underlying <see cref="Microsoft.AspNetCore.Components.Forms.EditContext"/>/message store, and
/// the validity/revalidation helpers built on top of them.
/// </summary>
public class HBEditContextTests
{
    private sealed class Person
    {
        public string? Name { get; set; }
    }

    [Fact]
    public void Context_IsCreatedForModel_AndCachedAcrossAccesses()
    {
        var editContext = new HBEditContext<Person>(new Person());

        Assert.Same(editContext.Context, editContext.Context);
    }

    [Fact]
    public void IsValid_IsTrue_WhenNoValidationMessagesHaveBeenAdded()
    {
        var editContext = new HBEditContext<Person>(new Person());

        Assert.True(editContext.IsValid());
    }

    [Fact]
    public void IsValid_IsFalse_AfterACustomErrorIsAdded()
    {
        var editContext = new HBEditContext<Person>(new Person());

        editContext.ValidationErrors.Add(x => x.Name, "Name is required.");

        Assert.False(editContext.IsValid());
    }

    [Fact]
    public void Revalidate_ClearsPreviouslyAddedCustomErrors()
    {
        var editContext = new HBEditContext<Person>(new Person());
        editContext.ValidationErrors.Add(x => x.Name, "Name is required.");

        editContext.Revalidate();

        Assert.True(editContext.IsValid());
    }
}

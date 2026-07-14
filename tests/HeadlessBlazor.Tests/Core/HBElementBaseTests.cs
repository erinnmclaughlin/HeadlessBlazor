namespace HeadlessBlazor.Tests.Core;

/// <summary>
/// Layer 2 (rendered component) tests for <see cref="HBElementBase"/>, exercised through the
/// concrete <see cref="HBElement"/>. bUnit renders to a real, queryable DOM but stubs
/// <c>IJSRuntime</c>, so the shared rendering pipeline is covered without any JS behavior running.
/// </summary>
public class HBElementBaseTests : BunitContext
{
    [Fact]
    public void RendersDivByDefault()
    {
        var cut = Render<HBElement>();

        Assert.NotNull(cut.Find("div"));
    }

    [Fact]
    public void RendersConfiguredElementName()
    {
        var cut = Render<CustomElement>();

        Assert.NotNull(cut.Find("section"));
    }

    [Fact]
    public void PassesThroughUnmatchedAttributes()
    {
        var cut = Render<HBElement>(ps => ps
            .AddUnmatched("data-foo", "bar")
            .AddUnmatched("id", "widget"));

        var el = cut.Find("div");
        Assert.Equal("bar", el.GetAttribute("data-foo"));
        Assert.Equal("widget", el.GetAttribute("id"));
    }

    [Fact]
    public void SkipsAttributesWithNullValue()
    {
        var cut = Render<HBElement>(ps => ps.AddUnmatched("data-foo", null));

        Assert.False(cut.Find("div").HasAttribute("data-foo"));
    }

    [Fact]
    public void RendersChildContent()
    {
        var cut = Render<HBElement>(ps => ps.AddChildContent("<span>hi</span>"));

        Assert.Equal("hi", cut.Find("span").TextContent);
    }

    [Fact]
    public void OnClickPreventDefault_IsSetFromInternalAttribute()
    {
        var cut = Render<HBElement>(ps => ps.AddUnmatched("__internal_preventDefault_onclick", true));

        Assert.True(cut.Instance.OnClickPreventDefault);
    }

    [Fact]
    public void OnClickStopPropagation_IsSetFromInternalAttribute()
    {
        var cut = Render<HBElement>(ps => ps.AddUnmatched("__internal_stopPropagation_onclick", true));

        Assert.True(cut.Instance.OnClickStopPropagation);
    }

    private sealed class CustomElement : HBElement
    {
        public override string ElementName { get; set; } = "section";
    }
}

using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor.Tests.Toast;

/// <summary>
/// Layer 2 (rendered component) tests for <see cref="HBToastClose"/>. No JS interop is involved
/// here (dismissal is plain .NET), so these exercise the full render + click behavior via bUnit.
/// </summary>
public class HBToastCloseTests : BunitContext
{
    [Fact]
    public void Throws_WhenNotCascadedFromAToastBody()
    {
        Assert.Throws<InvalidOperationException>(() => Render<HBToastClose>());
    }

    [Fact]
    public void RendersButton_WithTypeButton()
    {
        var toast = new FakeToastInstance();

        var cut = RenderWithToast(toast);

        var button = cut.Find("button");
        Assert.Equal("button", button.GetAttribute("type"));
    }

    [Fact]
    public void Click_DismissesTheToast()
    {
        var toast = new FakeToastInstance();

        var cut = RenderWithToast(toast);
        cut.Find("button").Click();

        Assert.Equal(1, toast.DismissCallCount);
    }

    private IRenderedComponent<HBToastClose> RenderWithToast(FakeToastInstance toast)
    {
        return Render<CascadingValue<IToastInstance>>(ps => ps
            .Add(p => p.Value, toast)
            .AddChildContent<HBToastClose>()).FindComponent<HBToastClose>();
    }
}

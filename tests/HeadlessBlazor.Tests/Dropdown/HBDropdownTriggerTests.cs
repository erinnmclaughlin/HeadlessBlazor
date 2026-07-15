using Microsoft.AspNetCore.Components.Rendering;

namespace HeadlessBlazor.Tests.Dropdown;

/// <summary>
/// Layer 2 (rendered component) tests for <see cref="HBDropdownTrigger"/>, exercised through a
/// parent <see cref="HBDropdown"/> (with outside-click detection disabled, see
/// <see cref="HBDropdownTests"/>) so the cascaded <c>Dropdown</c> parameter is populated.
/// </summary>
public class HBDropdownTriggerTests : BunitContext
{
    [Fact]
    public void RendersButtonElement_ByDefault()
    {
        var cut = Render<HBDropdown>(ps => ps
            .Add(p => p.CloseOnOutsideClick, false)
            .Add(p => p.ChildContent, _ => (RenderTreeBuilder builder) =>
            {
                builder.OpenComponent<HBDropdownTrigger>(0);
                builder.CloseComponent();
            }));

        Assert.NotNull(cut.Find("button"));
    }

    [Fact]
    public void Click_TogglesTheDropdown()
    {
        var cut = Render<HBDropdown>(ps => ps
            .Add(p => p.CloseOnOutsideClick, false)
            .Add(p => p.ChildContent, _ => (RenderTreeBuilder builder) =>
            {
                builder.OpenComponent<HBDropdownTrigger>(0);
                builder.CloseComponent();
            }));

        cut.Find("button").Click();

        Assert.True(cut.Instance.IsOpen);
    }

    [Fact]
    public void Click_DoesNotToggleTheDropdown_WhenPreventDefaultIsSet()
    {
        var cut = Render<HBDropdown>(ps => ps
            .Add(p => p.CloseOnOutsideClick, false)
            .Add(p => p.ChildContent, _ => (RenderTreeBuilder builder) =>
            {
                builder.OpenComponent<HBDropdownTrigger>(0);
                builder.AddAttribute(1, nameof(HBDropdownTrigger.OnClickPreventDefault), true);
                builder.CloseComponent();
            }));

        cut.Find("button").Click();

        Assert.False(cut.Instance.IsOpen);
    }
}

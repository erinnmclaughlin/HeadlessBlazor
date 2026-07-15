using Microsoft.AspNetCore.Components.Rendering;

namespace HeadlessBlazor.Tests.Dropdown;

/// <summary>
/// Layer 2 (rendered component) tests for <see cref="HBDropdownItem"/>, exercised through a
/// parent <see cref="HBDropdown"/> (with outside-click detection disabled, see
/// <see cref="HBDropdownTests"/>) so the cascaded <c>Dropdown</c> parameter is populated.
/// </summary>
public class HBDropdownItemTests : BunitContext
{
    [Fact]
    public void Click_InvokesDropdownOnClickItem()
    {
        HBDropdownItem? clicked = null;
        var cut = Render<HBDropdown>(ps => ps
            .Add(p => p.CloseOnOutsideClick, false)
            .Add(p => p.OnClickItem, (HBDropdownItem item) => clicked = item)
            .Add(p => p.ChildContent, _ => (RenderTreeBuilder builder) =>
            {
                builder.OpenComponent<HBDropdownItem>(0);
                builder.AddAttribute(1, "id", "item");
                builder.CloseComponent();
            }));

        cut.Find("#item").Click();

        Assert.NotNull(clicked);
    }

    [Fact]
    public void Click_DoesNotInvokeDropdownOnClickItem_WhenStopPropagationIsSet()
    {
        var invoked = false;
        var cut = Render<HBDropdown>(ps => ps
            .Add(p => p.CloseOnOutsideClick, false)
            .Add(p => p.OnClickItem, (HBDropdownItem _) => invoked = true)
            .Add(p => p.ChildContent, _ => (RenderTreeBuilder builder) =>
            {
                builder.OpenComponent<HBDropdownItem>(0);
                builder.AddAttribute(1, "id", "item");
                builder.AddAttribute(2, nameof(HBDropdownItem.OnClickStopPropagation), true);
                builder.CloseComponent();
            }));

        cut.Find("#item").Click();

        Assert.False(invoked);
    }
}

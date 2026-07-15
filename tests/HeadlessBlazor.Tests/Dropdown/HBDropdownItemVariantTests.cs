namespace HeadlessBlazor.Tests.Dropdown;

/// <summary>
/// Layer 2 (rendered component) tests for the element-name overrides on
/// <see cref="HBDropdownItemLink"/> and <see cref="HBDropdownItemButton"/>. Rendered standalone
/// (no parent <see cref="HBDropdown"/>) since the <c>Dropdown</c> cascading parameter is only
/// needed once a click is handled, not for rendering.
/// </summary>
public class HBDropdownItemVariantTests : BunitContext
{
    [Fact]
    public void HBDropdownItemLink_RendersAnchorElement()
    {
        var cut = Render<HBDropdownItemLink>();

        Assert.NotNull(cut.Find("a"));
    }

    [Fact]
    public void HBDropdownItemButton_RendersButtonElement()
    {
        var cut = Render<HBDropdownItemButton>();

        Assert.NotNull(cut.Find("button"));
    }
}

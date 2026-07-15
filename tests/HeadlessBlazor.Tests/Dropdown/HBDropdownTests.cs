namespace HeadlessBlazor.Tests.Dropdown;

/// <summary>
/// Layer 2 (rendered component) tests for <see cref="HBDropdown"/>. Rendered with
/// <see cref="HBDropdown.CloseOnOutsideClick"/> disabled so opening it never mounts
/// <c>HBOutsideClickBehavior</c> (a JS-interop behavior bUnit's stubbed <c>IJSRuntime</c> cannot
/// service) - these tests exercise the open/close state machine, not JS interop.
/// </summary>
public class HBDropdownTests : BunitContext
{
    [Fact]
    public void IsOpen_DefaultsToFalse()
    {
        var cut = Render<HBDropdown>();

        Assert.False(cut.Instance.IsOpen);
    }

    [Fact]
    public async Task OpenAsync_SetsIsOpen_AndInvokesOnOpen()
    {
        var opened = 0;
        var cut = Render<HBDropdown>(ps => ps
            .Add(p => p.CloseOnOutsideClick, false)
            .Add(p => p.OnOpen, (HBDropdown _) => opened++));

        await cut.Instance.OpenAsync();

        Assert.True(cut.Instance.IsOpen);
        Assert.Equal(1, opened);
    }

    [Fact]
    public async Task CloseAsync_SetsIsOpenFalse_AndInvokesOnClose()
    {
        var closed = 0;
        var cut = Render<HBDropdown>(ps => ps
            .Add(p => p.CloseOnOutsideClick, false)
            .Add(p => p.OnClose, (HBDropdown _) => closed++));

        await cut.Instance.OpenAsync();
        await cut.Instance.CloseAsync();

        Assert.False(cut.Instance.IsOpen);
        Assert.Equal(1, closed);
    }

    [Fact]
    public async Task ToggleAsync_TogglesIsOpen()
    {
        var cut = Render<HBDropdown>(ps => ps.Add(p => p.CloseOnOutsideClick, false));

        await cut.Instance.ToggleAsync();
        Assert.True(cut.Instance.IsOpen);

        await cut.Instance.ToggleAsync();
        Assert.False(cut.Instance.IsOpen);
    }

    [Fact]
    public async Task OnClickItem_DefaultsToClosingTheDropdown_WhenNoHandlerProvided()
    {
        var cut = Render<HBDropdown>(ps => ps.Add(p => p.CloseOnOutsideClick, false));
        await cut.Instance.OpenAsync();

        await cut.Instance.OnClickItem.InvokeAsync(null!);

        Assert.False(cut.Instance.IsOpen);
    }
}

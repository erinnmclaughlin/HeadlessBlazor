using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor.Tests.OutsideClick;

/// <summary>
/// Layer 2 (rendered component) tests for <see cref="HBOutsideClickBehavior"/>. As noted in
/// CLAUDE.md, bUnit stubs <c>IJSRuntime</c> and cannot verify the real effects of JS-interop
/// behaviors, so these tests assert the .NET-side contract (disposal guard, the
/// <see cref="HBOutsideClickBehavior.NotifyClickOutside"/> callback) rather than driving the
/// component through its JS-dependent render lifecycle. The actual outside-click detection is
/// verified manually via the docs site.
/// </summary>
public class HBOutsideClickBehaviorTests
{
    [Fact]
    public async Task DisposeAsync_DoesNotThrow_WhenNeverInitialized()
    {
        var behavior = new HBOutsideClickBehavior();

        await behavior.DisposeAsync();
    }

    [Fact]
    public async Task NotifyClickOutside_InvokesOnClick()
    {
        var invoked = 0;
        var behavior = new HBOutsideClickBehavior
        {
            OnClick = EventCallback.Factory.Create(this, () => invoked++)
        };

        behavior.NotifyClickOutside();

        // NotifyClickOutside is `async void` (required by [JSInvokable]), so give its
        // continuation a chance to run before asserting.
        await Task.Delay(10);

        Assert.Equal(1, invoked);
    }
}

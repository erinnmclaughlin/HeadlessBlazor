namespace HeadlessBlazor.Tests.DarkMode;

/// <summary>
/// Layer 2 (rendered component) tests for <see cref="HBDarkModeBehavior"/>. As noted in
/// CLAUDE.md, bUnit stubs <c>IJSRuntime</c> and cannot verify the real effects of JS-interop
/// behaviors, so these tests assert the .NET-side contract (default state, disposal guard)
/// rather than driving the component through its JS-dependent render lifecycle. The
/// `prefers-color-scheme` detection and change notifications are verified manually via the
/// docs site.
/// </summary>
public class HBDarkModeBehaviorTests
{
    [Fact]
    public void IsDarkMode_DefaultsToFalse()
    {
        var behavior = new HBDarkModeBehavior();

        Assert.False(behavior.IsDarkMode);
    }

    [Fact]
    public async Task DisposeAsync_DoesNotThrow_WhenNeverInitialized()
    {
        var behavior = new HBDarkModeBehavior();

        await behavior.DisposeAsync();
    }
}

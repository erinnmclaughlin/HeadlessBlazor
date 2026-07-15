using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor.Tests.FloatingElement;

/// <summary>
/// Layer 2 (rendered component) tests for <see cref="HBFloatBehavior"/>. As noted in CLAUDE.md,
/// bUnit stubs <c>IJSRuntime</c> and cannot verify the real effects of JS-interop behaviors, so
/// this asserts the .NET-side disposal guard rather than driving the component through its
/// JS-dependent render lifecycle. Positioning itself is verified manually via the docs site.
/// </summary>
public class HBFloatBehaviorTests
{
    [Fact]
    public async Task DisposeAsync_DoesNotThrow_WhenNeverInitialized()
    {
        var behavior = new HBFloatBehavior { Anchor = default, Content = default };

        await behavior.DisposeAsync();
    }
}

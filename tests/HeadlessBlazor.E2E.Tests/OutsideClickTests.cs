using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace HeadlessBlazor.E2E.Tests;

/// <summary>
/// Drives <c>HBOutsideClickBehavior</c> in a real browser across every render mode. Unlike the bUnit
/// tests - which can only assert the .NET-side contract, because bUnit stubs <c>IJSRuntime</c> -
/// these exercise the actual JS document listener.
/// </summary>
[Collection(HarnessCollection.Name)]
public sealed class OutsideClickTests(HarnessFixture fixture)
{
    /// <summary>
    /// The <c>outside</c> element wraps the container, so a default center-of-element click would
    /// land on the container instead. Clicking into its padding keeps the click genuinely outside.
    /// </summary>
    private static readonly LocatorClickOptions ClickPadding =
        new() { Position = new() { X = 5, Y = 5 } };

    [Theory]
    [InlineData("server")]
    [InlineData("server-noprerender")]
    [InlineData("wasm")]
    [InlineData("wasm-noprerender")]
    public async Task ClickOutsideContainer_InvokesCallback(string mode)
    {
        var page = await NewPageAsync(mode);

        await WaitForBehaviorAsync(page);
        var before = await WaitForSettledCountAsync(page);

        await page.GetByTestId("outside").ClickAsync(ClickPadding);

        await Expect(page.GetByTestId("count")).ToHaveTextAsync((before + 1).ToString());
    }

    [Theory]
    [InlineData("server")]
    [InlineData("server-noprerender")]
    [InlineData("wasm")]
    [InlineData("wasm-noprerender")]
    public async Task ClickInsideContainer_DoesNotInvokeCallback(string mode)
    {
        var page = await NewPageAsync(mode);

        await WaitForBehaviorAsync(page);
        var before = await WaitForSettledCountAsync(page);

        await page.GetByTestId("container").ClickAsync();
        await Task.Delay(500);

        Assert.Equal(before, await ReadCountAsync(page));
    }

    /// <summary>
    /// Under static SSR there is no interactivity, so the behavior must be inert rather than throw -
    /// the page still has to render.
    /// </summary>
    [Fact]
    public async Task StaticSsr_RendersScenarioButBehaviorIsInert()
    {
        var page = await NewPageAsync("static");

        await Expect(page.GetByTestId("container")).ToBeVisibleAsync();
        await Expect(page.GetByTestId("count")).ToHaveTextAsync("0");

        await page.GetByTestId("outside").ClickAsync(ClickPadding);
        await Task.Delay(500);

        Assert.Equal(0, await ReadCountAsync(page));
    }

    private async Task<IPage> NewPageAsync(string mode)
    {
        var context = await fixture.Browser.NewContextAsync();
        var page = await context.NewPageAsync();
        await page.GotoAsync($"{fixture.BaseUrl}/harness/{mode}/outside-click");
        return page;
    }

    private static async Task<int> ReadCountAsync(IPage page) =>
        int.Parse(await page.GetByTestId("count").InnerTextAsync());

    /// <summary>
    /// Blocks until an outside click actually registers.
    /// </summary>
    /// <remarks>
    /// Waiting for the element to appear is not enough: the component renders, and only then does
    /// the interop chain (dynamic <c>import</c> then <c>createInstance</c>) resolve and attach the
    /// document listener. A click landing in that gap is silently dropped, and nothing on the .NET
    /// side signals when it closes - so poll with clicks until one takes.
    /// </remarks>
    private static async Task WaitForBehaviorAsync(IPage page)
    {
        var outside = page.GetByTestId("outside");
        var deadline = DateTime.UtcNow + TimeSpan.FromSeconds(30);

        while (DateTime.UtcNow < deadline)
        {
            await outside.ClickAsync(ClickPadding);

            if (await ReadCountAsync(page) > 0)
            {
                return;
            }

            await Task.Delay(250);
        }

        throw new TimeoutException(
            "HBOutsideClickBehavior never registered an outside click; the page likely never became interactive.");
    }

    /// <summary>
    /// Blocks until the counter stops moving, and returns its resting value.
    /// </summary>
    /// <remarks>
    /// <see cref="WaitForBehaviorAsync"/> returns the moment a click registers, but under Blazor
    /// Server the clicks it already dispatched are still round-tripping over SignalR and will keep
    /// bumping the counter afterwards. Reading the baseline before that backlog drains makes the
    /// subsequent assertion race - invisibly under WebAssembly, where the round trip is instant.
    /// </remarks>
    private static async Task<int> WaitForSettledCountAsync(IPage page)
    {
        var last = await ReadCountAsync(page);
        var deadline = DateTime.UtcNow + TimeSpan.FromSeconds(15);

        while (DateTime.UtcNow < deadline)
        {
            await Task.Delay(500);
            var current = await ReadCountAsync(page);

            if (current == last)
            {
                return current;
            }

            last = current;
        }

        throw new TimeoutException("The outside-click counter never settled.");
    }
}

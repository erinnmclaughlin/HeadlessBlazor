namespace HeadlessBlazor.Tests.Toast;

/// <summary>
/// Layer 1 (pure logic) tests for <see cref="ToastService"/>: instance tracking, the
/// <c>StateChanged</c> signal, auto-dismiss, and the double-dismiss guard. No rendering and no
/// JS interop is involved, so these run as plain unit tests against the service directly.
/// </summary>
public class ToastServiceTests
{
    private static ToastService CreateService(ToastOptions? defaults = null) => new(defaults ?? new ToastOptions { Duration = null });

    [Fact]
    public void Show_AddsInstance_AndRaisesStateChanged()
    {
        var service = CreateService();
        var raised = 0;
        service.StateChanged += () => raised++;

        service.Show<TestToast>();

        Assert.Single(service.Instances);
        Assert.Equal(1, raised);
    }

    [Fact]
    public void Show_FallsBackToDefaultOptions_WhenNoneProvided()
    {
        var defaults = new ToastOptions { Duration = null };
        var service = CreateService(defaults);

        service.Show<TestToast>();

        Assert.Same(defaults, service.Instances[0].Options);
    }

    [Fact]
    public void Show_UsesProvidedOptions_OverDefaults()
    {
        var service = CreateService();
        var options = new ToastOptions { Duration = null };

        service.Show<TestToast>(options);

        Assert.Same(options, service.Instances[0].Options);
    }

    [Fact]
    public void Instances_AreOrderedOldestFirst()
    {
        var service = CreateService();

        service.Show<TestToast>();
        _ = service.Create<TestToast>().WithParam(x => x.Message, "second").Show();

        Assert.Equal(2, service.Instances.Count);
        Assert.False(service.Instances[0].Parameters.ContainsKey("Message"));
        Assert.Equal("second", service.Instances[1].Parameters["Message"]);
    }

    [Fact]
    public async Task DismissAsync_RemovesInstance_AndRaisesStateChanged_WhenTransitionsDisabled()
    {
        var service = CreateService();
        var instance = service.Show<TestToast>();
        var raised = 0;
        service.StateChanged += () => raised++;

        await instance.DismissAsync();

        Assert.Empty(service.Instances);
        Assert.Equal(1, raised);
    }

    [Fact]
    public async Task DismissAsync_MarksLeaving_ThenRemoves_WhenTransitionsEnabled()
    {
        var service = CreateService(new ToastOptions { Duration = null, TransitionDuration = TimeSpan.FromMilliseconds(1) });
        var instance = service.Show<TestToast>();
        var toastInstance = (ToastInstance)instance;

        var dismissTask = instance.DismissAsync();

        // Still mounted (in its "leaving" phase) until the transition duration elapses.
        Assert.Single(service.Instances);
        Assert.Equal("closed", toastInstance.DataState);

        await dismissTask;

        Assert.Empty(service.Instances);
    }

    [Fact]
    public async Task DismissAsync_Twice_IsNoOp()
    {
        var service = CreateService();
        var instance = service.Show<TestToast>();

        await instance.DismissAsync();
        await instance.DismissAsync();

        Assert.Empty(service.Instances);
    }
}

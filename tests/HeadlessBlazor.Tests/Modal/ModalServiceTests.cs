namespace HeadlessBlazor.Tests.Modal;

/// <summary>
/// Layer 1 (pure logic) tests for <see cref="ModalService"/>: instance tracking, the
/// <c>StateChanged</c> signal, result resolution, and the double-close guard. No rendering and no
/// JS interop is involved, so these run as plain unit tests against the service directly.
/// </summary>
public class ModalServiceTests
{
    private static ModalService CreateService(ModalOptions? defaults = null) => new(defaults ?? new ModalOptions());

    [Fact]
    public void ShowAsync_AddsInstance_AndRaisesStateChanged()
    {
        var service = CreateService();
        var raised = 0;
        service.StateChanged += () => raised++;

        _ = service.ShowAsync<TestModal, bool>();

        Assert.Single(service.Instances);
        Assert.Equal(1, raised);
    }

    [Fact]
    public void ShowAsync_FallsBackToDefaultOptions_WhenNoneProvided()
    {
        var defaults = new ModalOptions();
        var service = CreateService(defaults);

        _ = service.ShowAsync<TestModal, bool>();

        Assert.Same(defaults, service.Instances[0].Options);
    }

    [Fact]
    public void ShowAsync_UsesProvidedOptions_OverDefaults()
    {
        var service = CreateService();
        var options = new ModalOptions { CloseOnEscape = false };

        _ = service.ShowAsync<TestModal, bool>(options);

        Assert.Same(options, service.Instances[0].Options);
    }

    [Fact]
    public void Instances_AreOrderedOldestFirst()
    {
        var service = CreateService();

        _ = service.ShowAsync<TestModal, bool>();
        _ = service.Create<TestModal, bool>().WithParam(x => x.Title, "second").ShowAsync();

        Assert.Equal(2, service.Instances.Count);
        Assert.False(service.Instances[0].Parameters.ContainsKey("Title"));
        Assert.Equal("second", service.Instances[1].Parameters["Title"]);
    }

    [Fact]
    public async Task CloseAsync_ResolvesResultWithData_AndRemovesInstance()
    {
        var service = CreateService();
        var resultTask = service.ShowAsync<TestModal, bool>();
        var instance = (IModalInstance<bool>)service.Instances[0];

        await instance.CloseAsync(true);

        Assert.Empty(service.Instances);
        var result = await resultTask;
        Assert.False(result.Canceled);
        Assert.True(result.Data);
    }

    [Fact]
    public async Task CancelAsync_ResolvesCanceledResult_AndRemovesInstance()
    {
        var service = CreateService();
        var resultTask = service.ShowAsync<TestModal, bool>();
        var instance = service.Instances[0];

        await instance.CancelAsync();

        Assert.Empty(service.Instances);
        var result = await resultTask;
        Assert.True(result.Canceled);
        Assert.False(result.Data);
    }

    [Fact]
    public async Task Close_RaisesStateChanged()
    {
        var service = CreateService();
        var resultTask = service.ShowAsync<TestModal, bool>();
        var raised = 0;
        service.StateChanged += () => raised++;

        await ((IModalInstance<bool>)service.Instances[0]).CloseAsync(true);
        await resultTask;

        Assert.Equal(1, raised);
    }

    [Fact]
    public async Task CloseAsync_Twice_IsNoOp_AndFirstResultWins()
    {
        var service = CreateService();
        var resultTask = service.ShowAsync<TestModal, bool>();
        var instance = (IModalInstance<bool>)service.Instances[0];

        await instance.CloseAsync(true);
        // The instance is already gone; the guard in ModalService.CloseAsync must swallow this
        // rather than throwing or overwriting the resolved result.
        await instance.CloseAsync(false);

        var result = await resultTask;
        Assert.True(result.Data);
    }
}

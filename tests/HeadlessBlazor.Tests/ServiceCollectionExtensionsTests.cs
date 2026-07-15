using Microsoft.Extensions.DependencyInjection;

namespace HeadlessBlazor.Tests;

/// <summary>
/// Layer 1 (pure logic) tests for the bundle package's <see cref="ServiceCollectionExtensions.AddHeadlessBlazor"/>,
/// which just forwards to each component package's own DI registration.
/// </summary>
public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddHeadlessBlazor_RegistersModalService()
    {
        var services = new ServiceCollection();
        services.AddHeadlessBlazor();
        var provider = services.BuildServiceProvider();

        Assert.NotNull(provider.GetService<IModalService>());
    }

    [Fact]
    public void AddHeadlessBlazor_RegistersToastService()
    {
        var services = new ServiceCollection();
        services.AddHeadlessBlazor();
        var provider = services.BuildServiceProvider();

        Assert.NotNull(provider.GetService<IToastService>());
    }

    [Fact]
    public void AddHeadlessBlazor_AppliesConfiguredModalDefaults()
    {
        var services = new ServiceCollection();
        services.AddHeadlessBlazor(configureModalDefaults: options => options.CloseOnEscape = false);
        var provider = services.BuildServiceProvider();
        var modalService = (ModalService)provider.GetRequiredService<IModalService>();

        _ = modalService.ShowAsync<TestModal, bool>();

        Assert.False(modalService.Instances[0].Options.CloseOnEscape);
    }
}

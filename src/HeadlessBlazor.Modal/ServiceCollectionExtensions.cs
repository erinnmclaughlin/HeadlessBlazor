using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HeadlessBlazor;

/// <summary>
/// DI registration for <see cref="IModalService"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IModalService"/> as a scoped service.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="configureDefaults">
    /// Optional configuration for the default <see cref="ModalOptions"/> applied to every modal opened
    /// without its own options (e.g. shared overlay/dialog attributes or escape/outside-click behavior).
    /// </param>
    public static IServiceCollection AddHeadlessBlazorModal(this IServiceCollection services, Action<ModalOptions>? configureDefaults = null)
    {
        var defaultOptions = new ModalOptions();
        configureDefaults?.Invoke(defaultOptions);

        services.TryAddScoped<ModalService>(_ => new ModalService(defaultOptions));
        services.TryAddScoped<IModalService>(sp => sp.GetRequiredService<ModalService>());
        return services;
    }
}

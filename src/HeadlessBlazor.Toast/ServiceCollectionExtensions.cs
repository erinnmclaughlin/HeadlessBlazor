using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HeadlessBlazor;

/// <summary>
/// DI registration for <see cref="IToastService"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IToastService"/> as a scoped service.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="configureDefaults">
    /// Optional configuration for the default <see cref="ToastOptions"/> applied to every toast shown
    /// without its own options (e.g. shared attributes or duration).
    /// </param>
    public static IServiceCollection AddHeadlessBlazorToast(this IServiceCollection services, Action<ToastOptions>? configureDefaults = null)
    {
        var defaultOptions = new ToastOptions();
        configureDefaults?.Invoke(defaultOptions);

        services.TryAddScoped<ToastService>(_ => new ToastService(defaultOptions));
        services.TryAddScoped<IToastService>(sp => sp.GetRequiredService<ToastService>());
        return services;
    }
}

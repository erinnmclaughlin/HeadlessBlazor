using Microsoft.Extensions.DependencyInjection;

namespace HeadlessBlazor;

/// <summary>
/// Extension methods for configuring HeadlessBlazor services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the required HeadlessBlazor services.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configureModalDefaults">
    /// Optional configuration for the default <see cref="ModalOptions"/> applied to every modal opened
    /// without its own options.
    /// </param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddHeadlessBlazor(this IServiceCollection services, Action<ModalOptions>? configureModalDefaults = null)
    {
        services.AddHeadlessBlazorModal(configureModalDefaults);
        return services;
    }
}

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
    public static IServiceCollection AddHeadlessBlazorModal(this IServiceCollection services)
    {
        services.TryAddScoped<ModalService>();
        services.TryAddScoped<IModalService>(sp => sp.GetRequiredService<ModalService>());
        return services;
    }
}

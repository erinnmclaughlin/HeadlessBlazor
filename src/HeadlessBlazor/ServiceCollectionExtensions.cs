using HeadlessBlazor.Core.Themes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HeadlessBlazor;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHeadlessBlazor(this IServiceCollection services, Action<HBThemeBuilder>? themeBuilderAction = null)
    {
        services.TryAddSingleton<HBThemeFactory>();

        var themeBuilder = HBTheme.CreateBuilder();
        themeBuilderAction?.Invoke(themeBuilder);
        services.TryAddSingleton(themeBuilder.Build());

        return services;
    }
}

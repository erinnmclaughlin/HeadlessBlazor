using HeadlessBlazor.Core.Themes;
using Microsoft.Extensions.DependencyInjection;

namespace HeadlessBlazor;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHeadlessBlazor(this IServiceCollection services, Action<HBThemeBuilder>? themeBuilderAction = null)
    {
        var themeBuilder = HBTheme.CreateBuilder();
        themeBuilderAction?.Invoke(themeBuilder);

        services.AddSingleton(themeBuilder.Build());

        return services;
    }
}

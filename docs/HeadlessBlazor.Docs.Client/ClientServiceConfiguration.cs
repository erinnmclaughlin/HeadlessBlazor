using Blazored.LocalStorage;
using HeadlessBlazor.Themes.Bootstrap;
using HeadlessBlazor.Themes.Tailwind;

namespace HeadlessBlazor.Docs.Client;

public static class ClientServiceConfiguration
{
    public static IServiceCollection AddClientServices(this IServiceCollection services)
    {
        return services
            .AddHeadlessBlazor()
            .AddBootstrapTheme()
            .AddTailwindTheme()
            .AddBlazoredLocalStorage();
    }
}

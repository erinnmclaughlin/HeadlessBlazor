using HeadlessBlazor.Core.Themes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HeadlessBlazor.Themes.Bootstrap;

public static class HBBootstrapThemeConfiguration
{
    public static IServiceCollection AddBootstrapTheme(this IServiceCollection services)
    {
        var bootstrap = HBTheme.CreateBuilder().UseBootstrap();
        services.AddKeyedSingleton("Bootstrap", bootstrap);
        services.TryAddSingleton(sp => sp.GetRequiredKeyedService<HBTheme>("Bootstrap"));
        return services;
    }

    public static HBTheme UseBootstrap(this HBThemeBuilder t)
    {
        t.AddComponentDefaults<HBDropdown>(c => c.AddDefaultClassNames("dropdown", "d-inline-block"));
        t.AddComponentDefaults<HBDropdownTrigger>(c => c.AddDefaultClassNames("btn", "btn-primary", "dropdown-toggle"));
        t.AddComponentDefaults<HBDropdownItems>(c => c.AddDefaultClassNames("dropdown-menu", "show"));
        t.AddComponentDefaults<HBDropdownItem>(c => c.AddDefaultClassNames("dropdown-item"));
        t.AddComponentDefaults<HBDropdownItemButton>(c =>
        {
            c.AddDefaultClassNames("dropdown-item");
            c.AddAttributeDefaults("type", "button");
        });
        t.AddComponentDefaults<HBDropdownItemLink>(c =>
        {
            c.AddDefaultClassNames("dropdown-item");
            c.AddAttributeDefaults("href", "#");
        });

        return t.Build();
    }
}

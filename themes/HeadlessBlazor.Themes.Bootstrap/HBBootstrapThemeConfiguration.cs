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
        t.AddComponentDefaults<Dropdown>(c => c.AddDefaultClassNames("dropdown", "d-inline-block"));
        t.AddComponentDefaults<DropdownTrigger>(c => c.AddDefaultClassNames("btn", "btn-primary", "dropdown-toggle"));
        t.AddComponentDefaults<DropdownItems>(c => c.AddDefaultClassNames("dropdown-menu", "show"));
        t.AddComponentDefaults<DropdownItem>(c => c.AddDefaultClassNames("dropdown-item"));
        t.AddComponentDefaults<DropdownItemButton>(c =>
        {
            c.AddDefaultClassNames("dropdown-item");
            c.AddAttributeDefaults("type", "button");
        });
        t.AddComponentDefaults<DropdownItemLink>(c =>
        {
            c.AddDefaultClassNames("dropdown-item");
            c.AddAttributeDefaults("href", "#");
        });

        return t.Build();
    }
}

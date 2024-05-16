using HeadlessBlazor.Core.Themes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HeadlessBlazor.Themes.Tailwind;

public static class HBTailwindThemeConfiguration
{
    public static IServiceCollection AddTailwindTheme(this IServiceCollection services)
    {
        var theme = HBTheme.CreateBuilder().UseTailwind();
        services.AddKeyedSingleton("Tailwind", theme);
        services.TryAddSingleton(sp => sp.GetRequiredKeyedService<HBTheme>("Tailwind"));
        return services;
    }

    public static HBTheme UseTailwind(this HBThemeBuilder t)
    {
        t.AddComponentDefaults<HBDropdown>(c => c.AddDefaultClassNames("relative", "inline-block", "text-left"));
        t.AddComponentDefaults<HBDropdownTrigger>(c =>
        {
            c.AddDefaultClassNames("inline-flex w-full justify-center gap-x-1.5 rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 [&:not(:disabled)]:hover:bg-gray-50");
            c.AddAttributeDefaults("type", "button");
        });
        t.AddComponentDefaults<HBDropdownItems>(c =>
        {
            c.AddDefaultClassNames("absolute left-0 z-10 mt-2 w-56 origin-top-right rounded-md bg-white shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none");
        });
        t.AddComponentDefaults<HBDropdownItem>(c => c.AddDefaultClassNames("text-gray-700 block px-4 py-2 text-sm text-left w-full hover:text-gray-900 hover:bg-gray-100"));
        t.AddComponentDefaults<HBDropdownItemButton>(c =>
        {
            c.AddDefaultClassNames("text-gray-700 block px-4 py-2 text-sm text-left w-full [&:not(:disabled)]:hover:text-gray-900 [&:not(:disabled)]:hover:bg-gray-100 disabled:opacity-50");
            c.AddAttributeDefaults("type", "button");
        });
        t.AddComponentDefaults<HBDropdownItemLink>(c =>
        {
            c.AddDefaultClassNames("text-gray-700 block px-4 py-2 text-sm text-left w-full hover:text-gray-900 hover:bg-gray-100");
            c.AddAttributeDefaults("href", "#");
        });

        return t.Build();
    }
}

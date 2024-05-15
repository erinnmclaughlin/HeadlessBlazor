using HeadlessBlazor.Core.Themes;

namespace HeadlessBlazor.Themes.Bootstrap;

public static class HBBootstrapThemeConfiguration
{
    public static void UseBootstrap(this HBThemeBuilder t)
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
    }
}

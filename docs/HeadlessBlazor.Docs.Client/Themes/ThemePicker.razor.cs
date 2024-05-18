using HeadlessBlazor.Core.Themes;
using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor.Docs.Client.Themes;

public sealed partial class ThemePicker
{
    private string CurrentTheme { get; set; } = "";

    [CascadingParameter]
    private HBThemeFactory? ThemeFactory { get; set; }

    protected override void OnParametersSet()
    {
        CurrentTheme = ThemeFactory?.CurrentTheme ?? "";
    }

    private void AfterThemeUpdated()
    {
        Console.WriteLine($"Changing theme to '{CurrentTheme}'");
        ThemeFactory?.SetTheme(CurrentTheme);
    }
}

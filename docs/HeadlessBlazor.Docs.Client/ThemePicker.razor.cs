using HeadlessBlazor.Core.Themes;
using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor.Docs.Client;

public sealed partial class ThemePicker : IDisposable
{
    [Inject]
    private HBThemeFactory ThemeFactory { get; set; } = default!;

    public void Dispose()
    {
        ThemeFactory.ThemeChanged -= OnThemeChanged;
    }

    protected override void OnInitialized()
    {
        ThemeFactory.ThemeChanged += OnThemeChanged;
    }

    private void OnThemeChanged(string theme)
    {
        InvokeAsync(StateHasChanged);
    }
}

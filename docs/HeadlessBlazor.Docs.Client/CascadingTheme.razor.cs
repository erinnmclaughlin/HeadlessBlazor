using HeadlessBlazor.Core.Themes;
using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor.Docs.Client;

public sealed partial class CascadingTheme : IDisposable
{
    [Inject]
    private HBThemeFactory ThemeFactory { get; set; } = default!;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

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

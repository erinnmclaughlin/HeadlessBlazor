using Blazored.LocalStorage;
using HeadlessBlazor.Core.Themes;
using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor.Docs.Client.Themes;

public sealed partial class ThemeState : IDisposable
{
    private bool IsRendered { get; set; }

    [Inject]
    private ILocalStorageService LocalStorage { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private HBThemeFactory ThemeFactory { get; set; } = default!;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    public void Dispose()
    {
        ThemeFactory.ThemeChanged -= OnThemeChanged;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            ThemeFactory.CurrentTheme = await GetSavedThemeAsync();
            ThemeFactory.ThemeChanged += OnThemeChanged;
            IsRendered = true;
            StateHasChanged();
        }
    }

    private void OnThemeChanged(string theme)
    {
        InvokeAsync(async () =>
        {
            var savedTheme = await GetSavedThemeAsync();

            if (savedTheme.Equals(theme, StringComparison.OrdinalIgnoreCase))
                return;

            await LocalStorage.SetItemAsync("theme", theme);
            NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
        });
    }

    private async Task<string> GetSavedThemeAsync()
    {
        return await LocalStorage.GetItemAsync<string>("theme") ?? "";
    }
}

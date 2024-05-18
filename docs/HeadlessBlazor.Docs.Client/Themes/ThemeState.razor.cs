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
            var savedTheme = await GetSavedThemeAsync();
            ThemeFactory.SetTheme(savedTheme);

            ThemeFactory.ThemeChanged += OnThemeChanged;

            IsRendered = true;
            StateHasChanged();
        }
    }

    private void OnThemeChanged(string theme)
    {
        if (!IsRendered) return;

        Console.WriteLine($"Theme changed to {theme}!");
        Exception? exception = null;
        InvokeAsync(async () =>
        {
            try
            {
                var savedTheme = await GetSavedThemeAsync();

                Console.WriteLine("saved theme is " + savedTheme);

                if (savedTheme.Equals(theme, StringComparison.OrdinalIgnoreCase))
                    return;

                Console.WriteLine("Saving to local storage...");
                await LocalStorage.SetItemAsync("theme", theme);

                Console.WriteLine("Saved!");
                NavigationManager.NavigateTo(NavigationManager.BaseUri, forceLoad: true, replace: true);
                Console.WriteLine("Reloading!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                exception = ex;
            }
        });

        if (exception is not null)
            throw exception;
    }

    private async Task<string> GetSavedThemeAsync()
    {
        return await LocalStorage.GetItemAsync<string>("theme") ?? "";
    }
}

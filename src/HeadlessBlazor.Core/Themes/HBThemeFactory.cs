using Microsoft.Extensions.DependencyInjection;

namespace HeadlessBlazor.Core.Themes;

public sealed class HBThemeFactory(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public event Action<string>? ThemeChanged;

    private string _theme = "";
    public string CurrentTheme
    {
        get => _theme;
        private set
        {
            _theme = value;
            ThemeChanged?.Invoke(value);
        }
    }

    public HBTheme GetTheme() => _serviceProvider.GetKeyedService<HBTheme>(CurrentTheme) ?? _serviceProvider.GetRequiredService<HBTheme>();
    public void SetTheme(string theme) => CurrentTheme = theme;
}

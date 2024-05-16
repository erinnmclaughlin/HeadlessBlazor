﻿using Microsoft.Extensions.DependencyInjection;

namespace HeadlessBlazor.Core.Themes;

public sealed class HBThemeFactory(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public event Action<string>? ThemeChanged;

    private string _theme = "";
    public string Theme
    {
        get => _theme;
        set
        {
            _theme = value;
            ThemeChanged?.Invoke(value);
        }
    }

    public HBTheme GetTheme() => _serviceProvider.GetKeyedService<HBTheme>(Theme) ?? _serviceProvider.GetRequiredService<HBTheme>();
    public HBTheme GetTheme(string theme) => _serviceProvider.GetRequiredKeyedService<HBTheme>(theme);
}
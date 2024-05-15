namespace HeadlessBlazor.Core.Themes;

public class HBTheme
{
    private readonly Dictionary<Type, HBThemeProvider> _themeProviders = [];

    public HBTheme() { }
    public HBTheme(IDictionary<Type, HBThemeProvider> themeProviders)
    {
        _themeProviders = themeProviders.ToDictionary();
    }

    public void ApplyDefaults<T>(T element) where T : HBElementBase
    {
        var elementType = element.GetType();

        if (_themeProviders.TryGetValue(elementType, out var provider))
        {
            provider.ApplyDefaults(element);
        }
    }

    public static HBThemeBuilder CreateBuilder() => new();
}

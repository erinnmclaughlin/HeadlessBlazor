namespace HeadlessBlazor.Core.Themes;

public class HBThemeBuilder
{
    private readonly Dictionary<Type, HBThemeProvider> _elementThemes = [];

    public void AddComponentDefaults<T>(Action<HBThemeProviderBuilder<T>> configure) where T : HBElementBase
    {
        var provider = new HBThemeProviderBuilder<T>();
        configure(provider);
        _elementThemes.Add(typeof(T), provider.Build());
    }

    public HBTheme Build() => new(_elementThemes);
}

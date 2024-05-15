namespace HeadlessBlazor.Core.Themes;

public class HBThemeProviderBuilder<T>() where T : HBElementBase
{
    protected Action<HBElementBase> Action { get; set; } = _ => { };

    public HBThemeProvider Build()
    {
        return new(typeof(T), Action);
    }

    public HBThemeProviderBuilder<T> AddDefaultClassNames(params string[] classNames)
    {
        AddAttributeDefaults("class", string.Join(" ", classNames));
        return this;
    }

    public HBThemeProviderBuilder<T> AddDefaultStyle(IDictionary<string, string> styles)
    {
        AddAttributeDefaults("style", string.Join(";", styles.Select(x => $"{x.Key}:{x.Value}")));
        return this;
    }

    public HBThemeProviderBuilder<T> AddAttributeDefaults(string key, object? value)
    {
        Action += e =>
        {
            if (!e.UserAttributes.TryAdd(key, value))
                e.UserAttributes[key] = value;
        };

        return this;
    }
}

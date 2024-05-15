namespace HeadlessBlazor.Core.Themes;

public class HBThemeProvider(Type type, Action<HBElementBase> action)
{
    public Action<HBElementBase> Action { get; } = action;
    public Type Type { get; } = type;

    public void ApplyDefaults<T>(T element) where T : HBElementBase
    {
        if (element.GetType() == Type)
        {
            Action(element);
        }
    }
}

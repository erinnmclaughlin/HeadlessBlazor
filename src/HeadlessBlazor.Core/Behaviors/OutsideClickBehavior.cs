namespace HeadlessBlazor.Core.Behaviors;

public class HBOutsideClickBehavior : HBElement
{
    protected override void OnInitialized()
    {
        base.OnInitialized();

        UserAttributes.TryAdd("style", "position: fixed; top:0; right: 0; bottom: 0; left:0");
    }
}

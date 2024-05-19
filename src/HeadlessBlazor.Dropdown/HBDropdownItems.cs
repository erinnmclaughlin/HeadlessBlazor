using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor;

public class HBDropdownItems : HBElement
{
    [CascadingParameter]
    public HBDropdown Dropdown { get; set; } = default!;

    protected override void OnBeforeInitialized()
    {
        if (Dropdown == null)
        {
            throw new InvalidOperationException($"{GetType().Name} requires a cascading parameter of type {typeof(HBDropdown).Name}.");
        }
    }

    protected override void OnParametersSet()
    {
        UserAttributes.TryAdd("hb-popover", "");
    }
}

using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor;

public class HBDropdownTrigger : HBElement
{
    [CascadingParameter]
    public HBDropdown Dropdown { get; set; } = default!;

    [Parameter]
    public override string ElementName { get; set; } = "button";

    protected override void OnParametersSet()
    {
        UserAttributes.Add("hb-popover-anchor", "");
        UserAttributes.TryAdd("onclick", new EventCallback(this, Dropdown.ToggleAsync));
    }
}

using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor;

public class HBDropdownItem : HBElement
{
    [CascadingParameter]
    public HBDropdown Dropdown { get; set; } = default!;

    protected override void OnParametersSet()
    {
        UserAttributes.TryAdd("onclick", new EventCallback(this, HandleClick));
    }

    protected virtual async Task HandleClick()
    {
        if (!OnClickStopPropagation)
            await Dropdown.OnClickItem.InvokeAsync(this);
    }
}

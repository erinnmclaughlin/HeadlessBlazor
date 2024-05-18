using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor;

public class HBDropdownItem : HBElement
{
    [CascadingParameter]
    public HBDropdown Dropdown { get; set; } = default!;

    protected override void OnParametersSet()
    {
        if (Dropdown == null)
        {
            throw new InvalidOperationException($"{GetType().Name} requires a cascading parameter of type {typeof(HBDropdown).Name}.");
        }

        UserAttributes.TryAdd("onclick", new EventCallback(this, HandleClick));
    }

    protected virtual async Task HandleClick()
    {
        if (!OnClickStopPropagation)
            await Dropdown.OnClickItem.InvokeAsync(this);
    }
}

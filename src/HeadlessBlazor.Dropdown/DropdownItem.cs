using HeadlessBlazor.Core;
using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor;

public class DropdownItem : HBElement
{
    [CascadingParameter]
    public Dropdown Dropdown { get; set; } = default!;

    [Parameter]
    public bool InheritClickHandler { get; set; } = true;

    protected override void OnParametersSet()
    {
        if (Dropdown == null)
        {
            throw new InvalidOperationException($"{GetType().Name} requires a cascading parameter of type {typeof(Dropdown).Name}.");
        }

        UserAttributes.TryAdd("onclick", new EventCallback(this, HandleClick));
    }

    protected virtual async Task HandleClick()
    {
        if (InheritClickHandler)
            await Dropdown.OnClickItem.InvokeAsync(this);
    }
}

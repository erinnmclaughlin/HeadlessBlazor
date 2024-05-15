using HeadlessBlazor.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace HeadlessBlazor;

public class DropdownItem : HBElement
{
    [CascadingParameter]
    public Dropdown Dropdown { get; set; } = default!;

    protected override void OnParametersSet()
    {
        if (Dropdown == null)
        {
            throw new InvalidOperationException($"{GetType().Name} requires a cascading parameter of type {typeof(Dropdown).Name}.");
        }

        UserAttributes.TryAdd("onclick", new EventCallback(this, HandleClick));

        base.OnParametersSet();
    }

    protected virtual async Task HandleClick(MouseEventArgs e)
    {
        if (!OnClickStopPropagation)
            await Dropdown.OnClickItem.InvokeAsync(this);
    }
}

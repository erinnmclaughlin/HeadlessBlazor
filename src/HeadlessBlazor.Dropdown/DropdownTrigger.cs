using HeadlessBlazor.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace HeadlessBlazor;

public class DropdownTrigger : HBElement
{
    [CascadingParameter]
    public Dropdown Dropdown { get; set; } = default!;

    [Parameter]
    public override string ElementName { get; set; } = "button";

    [Parameter]
    public bool CloseOnEscape { get; set; } = true;

    [Parameter]
    public bool TriggerOnHover { get; set; }

    protected override void OnParametersSet()
    {
        if (Dropdown == null)
        {
            throw new InvalidOperationException($"{GetType().Name} requires a cascading parameter of type {typeof(Dropdown).Name}.");
        }

        UserAttributes.TryAdd("onclick", new EventCallback(this, HandleClick));
        UserAttributes.TryAdd("onkeydown", new EventCallback<KeyboardEventArgs>(this, HandleKeyDown));
    }

    private async Task HandleClick()
    {
        await Dropdown.Toggle();
    }
    
    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Code == "Escape" && CloseOnEscape)
            await Dropdown.Close();
    }
}

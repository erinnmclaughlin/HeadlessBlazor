using HeadlessBlazor.Core.Behaviors;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace HeadlessBlazor;

public class HBDropdownOutsideClickBehavior : HBOutsideClickBehavior
{
    [CascadingParameter]
    public HBDropdown Dropdown { get; set; } = default!;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (Dropdown.IsOpen)
            base.BuildRenderTree(builder);
    }

    protected override void OnParametersSet()
    {
        if (Dropdown == null)
        {
            throw new InvalidOperationException($"{GetType().Name} requires a cascading parameter of type {typeof(HBDropdown).Name}.");
        }

        UserAttributes.TryAdd("onclick", new EventCallback(this, HandleClick));
    }

    private async Task HandleClick()
    {
        await Dropdown.Close();
    }
}

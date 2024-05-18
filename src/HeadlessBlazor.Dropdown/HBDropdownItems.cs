using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace HeadlessBlazor;

public class HBDropdownItems : HBElement
{
    [CascadingParameter]
    public HBDropdown Dropdown { get; set; } = default!;

    protected override void OnParametersSet()
    {
        if (Dropdown == null)
        {
            throw new InvalidOperationException($"{GetType().Name} requires a cascading parameter of type {typeof(HBDropdown).Name}.");
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (Dropdown.IsOpen)
            base.BuildRenderTree(builder);
    }
}

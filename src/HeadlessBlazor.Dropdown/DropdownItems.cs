using HeadlessBlazor.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace HeadlessBlazor;

public class DropdownItems : HBElement
{
    [CascadingParameter]
    public Dropdown Dropdown { get; set; } = default!;

    protected override void OnParametersSet()
    {
        if (Dropdown == null)
        {
            throw new InvalidOperationException($"{GetType().Name} requires a cascading parameter of type {typeof(Dropdown).Name}.");
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (Dropdown.IsOpen)
            base.BuildRenderTree(builder);
    }
}

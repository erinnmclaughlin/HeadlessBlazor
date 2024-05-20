using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace HeadlessBlazor;

public class HBDropdownMenu : HBElement
{
    public ElementReference ElementReference { get; private set; }

    [Parameter]
    public HBFloatAlignment? Alignment { get; set; }

    [Parameter]
    public bool AutoPosition { get; set; } = true;

    [CascadingParameter]
    protected HBDropdown Dropdown { get; set; } = default!;

    [Parameter]
    public HBFloatSide? Side { get; set; }

    protected override void AddBehaviors(ref int sequence, RenderTreeBuilder builder)
    {
        if (AutoPosition)
        {
            builder.OpenComponent<HBFloatBehavior>(sequence++);
            builder.AddAttribute(sequence, nameof(HBFloatBehavior.Alignment), Alignment);
            builder.AddAttribute(sequence, nameof(HBFloatBehavior.Anchor), Dropdown.ElementReference);
            builder.AddAttribute(sequence, nameof(HBFloatBehavior.Content), ElementReference);
            builder.AddAttribute(sequence, nameof(HBFloatBehavior.Side), Side);
            builder.CloseComponent();
        }
    }

    protected override void AddElementReference(ref int sequence, RenderTreeBuilder builder)
    {
        builder.AddElementReferenceCapture(sequence, async (elementRef) =>
        {
            ElementReference = elementRef;
            await InvokeAsync(StateHasChanged);
        });
    }
}

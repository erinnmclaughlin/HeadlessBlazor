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

    protected override void AddBehaviors(RenderTreeBuilder builder, ref int sequenceNumber)
    {
        if (AutoPosition)
        {
            builder.OpenComponent<HBFloatBehavior>(sequenceNumber++);
            builder.AddAttribute(sequenceNumber++, nameof(HBFloatBehavior.Alignment), Alignment);
            builder.AddAttribute(sequenceNumber++, nameof(HBFloatBehavior.Anchor), Dropdown.ElementReference);
            builder.AddAttribute(sequenceNumber++, nameof(HBFloatBehavior.Content), ElementReference);
            builder.AddAttribute(sequenceNumber++, nameof(HBFloatBehavior.Side), Side);
            builder.CloseComponent();
        }
    }

    protected override void AddElementReference(RenderTreeBuilder builder, ref int sequenceNumber)
    {
        builder.AddElementReferenceCapture(sequenceNumber, async (elementRef) =>
        {
            ElementReference = elementRef;
            await InvokeAsync(StateHasChanged);
        });
    }
}

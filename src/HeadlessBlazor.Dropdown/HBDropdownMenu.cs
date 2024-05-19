using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace HeadlessBlazor;

public class HBDropdownMenu : HBElement
{
    public ElementReference ElementReference { get; private set; }

    [Parameter]
    public HBPopoverAlignment Alignment { get; set; } = HBPopoverAlignment.Start;

    [Parameter]
    public bool AutoPosition { get; set; } = true;

    [CascadingParameter]
    protected HBDropdown Dropdown { get; set; } = default!;

    [Parameter]
    public HBPopoverSide Side { get; set; } = HBPopoverSide.Bottom;

    protected override void AddBehaviors(RenderTreeBuilder builder, ref int sequenceNumber)
    {
        if (AutoPosition)
        {
            builder.OpenComponent<HBPopoverBehavior>(sequenceNumber++);
            builder.AddAttribute(sequenceNumber++, nameof(HBPopoverBehavior.Alignment), Alignment);
            builder.AddAttribute(sequenceNumber++, nameof(HBPopoverBehavior.Anchor), Dropdown.ElementReference);
            builder.AddAttribute(sequenceNumber++, nameof(HBPopoverBehavior.Content), ElementReference);
            builder.AddAttribute(sequenceNumber++, nameof(HBPopoverBehavior.Side), Side);
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

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace HeadlessBlazor;

/// <summary>
/// The dropdown menu.
/// </summary>
public class HBDropdownMenu : HBElement
{
    /// <summary>
    /// A reference to the HTML element.
    /// </summary>
    public ElementReference ElementReference { get; private set; }

    /// <summary>
    /// The dropdown menu alignment, relative to the <see cref="HBDropdownTrigger"/>.
    /// </summary>
    [Parameter]
    public HBFloatAlignment? Alignment { get; set; }

    /// <summary>
    /// When <see langword="true"/>, the dropdown menu will be automatically positioned based on the available UI space.
    /// Default is <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool AutoPosition { get; set; } = true;

    /// <summary>
    /// The parent <see cref="HBDropdown"/> component.
    /// </summary>
    [CascadingParameter]
    protected HBDropdown Dropdown { get; set; } = default!;

    /// <summary>
    /// The dropdown menu side, relative to the <see cref="HBDropdownTrigger"/>.
    /// </summary>
    [Parameter]
    public HBFloatSide? Side { get; set; }

    /// <inheritdoc/>
    protected override void OnBeforeCloseElement(ref int sequence, RenderTreeBuilder builder)
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

        builder.AddElementReferenceCapture(sequence, async (elementRef) =>
        {
            ElementReference = elementRef;
            await InvokeAsync(StateHasChanged);
        });

        base.OnBeforeCloseElement(ref sequence, builder);
    }
}

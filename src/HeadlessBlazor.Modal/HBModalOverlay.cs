using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace HeadlessBlazor;

/// <summary>
/// The modal backdrop. Relocated to the end of the document body via
/// <see cref="HBPortalBehavior"/> so it can cover the viewport regardless of where
/// the modal is declared. Clicking it closes the modal when
/// <see cref="HBModal.CloseOnOutsideClick"/> is enabled.
/// </summary>
public class HBModalOverlay : HBElement
{
    /// <summary>
    /// A reference to the HTML element.
    /// </summary>
    public ElementReference ElementReference { get; private set; }

    /// <summary>
    /// The parent <see cref="HBModal"/> component.
    /// </summary>
    [CascadingParameter]
    protected HBModal? Modal { get; set; }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        if (Modal?.CloseOnOutsideClick ?? false)
            UserAttributes.TryAdd("onclick", new EventCallback(this, Modal.CloseAsync));
    }

    /// <inheritdoc />
    protected override void OnBeforeCloseElement(ref int sequence, RenderTreeBuilder builder)
    {
        // The reference capture must be added immediately after the element's
        // attributes and before any child content (the behaviour component
        // below), otherwise the render tree diff fails when the element is removed.
        builder.AddElementReferenceCapture(sequence++, async capturedRef =>
        {
            ElementReference = capturedRef;
            await InvokeAsync(StateHasChanged);
        });

        builder.OpenComponent<HBPortalBehavior>(sequence++);
        builder.AddAttribute(sequence++, nameof(HBPortalBehavior.Element), ElementReference);
        builder.CloseComponent();

        base.OnBeforeCloseElement(ref sequence, builder);
    }
}

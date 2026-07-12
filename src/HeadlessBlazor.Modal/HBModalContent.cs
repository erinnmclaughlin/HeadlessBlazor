using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace HeadlessBlazor;

/// <summary>
/// The modal dialog panel. Applies dialog ARIA semantics, is relocated to the end
/// of the document body via <see cref="HBPortalBehavior"/>, and traps focus while
/// open via <see cref="HBFocusTrapBehavior"/>.
/// </summary>
public class HBModalContent : HBElement
{
    /// <summary>
    /// A reference to the HTML element.
    /// </summary>
    public ElementReference ElementReference { get; private set; }

    /// <summary>
    /// The <c>id</c> of the element that labels this dialog, registered by an
    /// <see cref="HBModalTitle"/>. Used to set <c>aria-labelledby</c>.
    /// </summary>
    public string? LabelledBy { get; private set; }

    /// <summary>
    /// The <c>id</c> of the element that describes this dialog, registered by an
    /// <see cref="HBModalDescription"/>. Used to set <c>aria-describedby</c>.
    /// </summary>
    public string? DescribedBy { get; private set; }

    /// <summary>
    /// The parent <see cref="HBModal"/> component.
    /// </summary>
    [CascadingParameter]
    protected HBModal Modal { get; set; } = default!;

    /// <summary>
    /// Registers the element that labels this dialog. Called by <see cref="HBModalTitle"/>.
    /// </summary>
    /// <param name="id">The <c>id</c> of the labelling element.</param>
    public void RegisterLabelledBy(string id)
    {
        LabelledBy = id;
        // Set via UserAttributes so the value participates in the stable, single-
        // sequence attribute block. Adding it with a dynamic AddAttribute call
        // instead would shift the sequence of the element reference capture that
        // follows, breaking the render tree diff.
        UserAttributes["aria-labelledby"] = id;
        StateHasChanged();
    }

    /// <summary>
    /// Registers the element that describes this dialog. Called by <see cref="HBModalDescription"/>.
    /// </summary>
    /// <param name="id">The <c>id</c> of the describing element.</param>
    public void RegisterDescribedBy(string id)
    {
        DescribedBy = id;
        UserAttributes["aria-describedby"] = id;
        StateHasChanged();
    }

    /// <inheritdoc />
    protected override void OnBeforeInitialized()
    {
        UserAttributes.TryAdd("role", "dialog");
        UserAttributes.TryAdd("aria-modal", "true");
        UserAttributes.TryAdd("tabindex", "-1");
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var seq = 0;
        builder.OpenComponent<CascadingValue<HBModalContent>>(seq++);
        builder.AddAttribute(seq++, "Value", this);
        builder.AddAttribute(seq++, "ChildContent", (RenderFragment)(b =>
        {
            var innerSeq = seq;
            BuildRenderTree(ref innerSeq, b);
        }));

        builder.CloseComponent();
    }

    /// <inheritdoc />
    protected override void OnBeforeCloseElement(ref int sequence, RenderTreeBuilder builder)
    {
        // aria-labelledby / aria-describedby are applied via UserAttributes (see
        // RegisterLabelledBy / RegisterDescribedBy) so their presence never shifts
        // the sequence numbers of the frames below.
        if (Modal.CloseOnEscape)
        {
            builder.AddAttribute(sequence++, "onkeydown", EventCallback.Factory.Create<KeyboardEventArgs>(this, async args =>
            {
                if (args.Key == "Escape")
                    await Modal.CloseAsync();
            }));
        }

        // The reference capture must be added immediately after the element's
        // attributes and before any child content (including the behaviour
        // components below), otherwise the render tree diff fails when the
        // element is removed.
        builder.AddElementReferenceCapture(sequence++, async capturedRef =>
        {
            ElementReference = capturedRef;
            await InvokeAsync(StateHasChanged);
        });

        builder.OpenComponent<HBPortalBehavior>(sequence++);
        builder.AddAttribute(sequence++, nameof(HBPortalBehavior.Element), ElementReference);
        builder.CloseComponent();

        builder.OpenComponent<HBFocusTrapBehavior>(sequence++);
        builder.AddAttribute(sequence++, nameof(HBFocusTrapBehavior.Element), ElementReference);
        builder.CloseComponent();

        base.OnBeforeCloseElement(ref sequence, builder);
    }
}

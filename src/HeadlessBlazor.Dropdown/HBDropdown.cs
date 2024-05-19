using HeadlessBlazor.Behaviors;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace HeadlessBlazor;

public class HBDropdown : HBElement<HBDropdown>, ICloseable, IReferenceable
{
    public bool IsElementReferenceSet { get; private set; }
    public ElementReference ElementReference { get; private set; }
    public bool IsOpen { get; private set; }

    [Parameter]
    public HBPopoverAlignment Alignment { get; set; } = HBPopoverAlignment.Start;

    [Parameter]
    public HBPopoverSide Side { get; set; } = HBPopoverSide.Bottom;

    [Parameter]
    public bool CloseOnOutsideClick { get; set; } = true;

    [Parameter]
    public EventCallback<HBDropdownItem> OnClickItem { get; set; }

    public Task ToggleAsync() => SetIsOpen(!IsOpen);
    public Task OpenAsync() => SetIsOpen(true);
    public Task CloseAsync() => SetIsOpen(false);

    protected override void OnAfterInitialized()
    {
        if (!OnClickItem.HasDelegate)
            OnClickItem = new EventCallback<HBDropdownItem>(this, CloseAsync);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var seq = 0;
        builder.OpenComponent<CascadingValue<HBDropdown>>(seq++);
        builder.AddAttribute(seq, "Value", this);
        builder.AddAttribute(seq++, "ChildContent", (RenderFragment)((b) =>
        {
            if (IsOpen)
            {
                b.OpenComponent<HBPopoverBehavior>(seq++);
                b.AddAttribute(seq++, nameof(HBPopoverBehavior.Alignment), Alignment);
                b.AddAttribute(seq++, nameof(HBPopoverBehavior.Container), this);
                b.AddAttribute(seq++, nameof(HBPopoverBehavior.Side), Side);
                b.CloseComponent();

                if (CloseOnOutsideClick)
                {
                    b.OpenComponent<HBOutsideClickBehavior>(seq++);
                    b.AddAttribute(seq++, nameof(HBOutsideClickBehavior.OnClick), new EventCallback(this, CloseAsync));
                    b.AddAttribute(seq++, nameof(HBOutsideClickBehavior.Container), this);
                    b.CloseComponent();
                }
            }

            BuildRenderTree(b, ref seq);
        }));
        
        builder.CloseComponent();
    }

    protected override void AddElementReference(RenderTreeBuilder builder, ref int sequenceNumber)
    {
        builder.AddElementReferenceCapture(sequenceNumber++, async capturedRef =>
        {
            ElementReference = capturedRef;
            await InvokeAsync(StateHasChanged);
        });
    }

    private async Task SetIsOpen(bool isOpen)
    {
        await InvokeAsync(() =>
        {
            IsOpen = isOpen;
            StateHasChanged();
        });
    }
}

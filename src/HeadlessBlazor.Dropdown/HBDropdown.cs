using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace HeadlessBlazor;

public class HBDropdown : HBElement<HBDropdown>, IReferenceable
{
    public ElementReference ElementReference { get; private set; }

    public bool IsOpen { get; private set; }

    [Parameter]
    public bool CloseOnOutsideClick { get; set; } = true;

    [Parameter]
    public EventCallback<HBDropdownItem> OnClickItem { get; set; }

    public async Task OpenAsync() => await InvokeAsync(() =>
    {
        IsOpen = true;
        StateHasChanged();
    });

    public async Task CloseAsync() => await InvokeAsync(() =>
    {
        IsOpen = false;
        StateHasChanged();
    });

    public async Task ToggleAsync()
    {
        await (IsOpen ? CloseAsync() : OpenAsync());
    }

    protected override void OnAfterInitialized()
    {
        if (!OnClickItem.HasDelegate)
            OnClickItem = new EventCallback<HBDropdownItem>(this, CloseAsync);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var seq = 0;
        builder.OpenComponent<CascadingValue<HBDropdown>>(seq++);
        builder.AddAttribute(seq++, "Value", this);
        builder.AddAttribute(seq++, "ChildContent", (RenderFragment)((b) =>
        {
            BuildRenderTree(b, ref seq);

            if (IsOpen && CloseOnOutsideClick)
            {
                b.OpenComponent<HBOutsideClickBehavior>(seq++);
                b.AddAttribute(seq++, nameof(HBOutsideClickBehavior.OnClick), EventCallback.Factory.Create(this, CloseAsync));
                b.AddAttribute(seq++, nameof(HBOutsideClickBehavior.Container), this);
                b.CloseComponent();
            }
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
}

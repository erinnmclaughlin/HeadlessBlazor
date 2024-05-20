using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace HeadlessBlazor;

public class HBDropdown : HBElement<HBDropdown>
{
    public ElementReference ElementReference { get; private set; }

    public bool IsOpen { get; private set; }

    [Parameter]
    public bool CloseOnEscape { get; set; } = true;

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
            BuildRenderTree(ref seq, b);

            if (IsOpen && CloseOnOutsideClick)
            {
                b.OpenComponent<HBOutsideClickBehavior>(seq++);
                b.AddAttribute(seq++, nameof(HBOutsideClickBehavior.OnClick), EventCallback.Factory.Create(this, CloseAsync));
                b.AddAttribute(seq++, nameof(HBOutsideClickBehavior.Container), ElementReference);
                b.CloseComponent();
            }
        }));
        
        builder.CloseComponent();
    }

    protected override void AddEventHandlers(ref int sequence, RenderTreeBuilder builder)
    {
        if (CloseOnEscape)
        {
            builder.AddAttribute(sequence++, "onkeydown", EventCallback.Factory.Create<KeyboardEventArgs>(this, async (args) =>
            {
                if (args.Key == "Escape")
                    await CloseAsync();
            }));
        }
    }

    protected override void AddElementReference(ref int sequence, RenderTreeBuilder builder)
    {
        builder.AddElementReferenceCapture(sequence++, async capturedRef =>
        {
            ElementReference = capturedRef;
            await InvokeAsync(StateHasChanged);
        });
    }
}

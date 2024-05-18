using HeadlessBlazor.Behaviors;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace HeadlessBlazor;

public class HBDropdown : HBElement<HBDropdown>, ICloseable
{
    public bool IsOpen { get; private set; }

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
            if (IsOpen && CloseOnOutsideClick)
            {
                b.OpenComponent<HBOutsideClickBehavior<HBDropdown>>(seq++);
                b.CloseComponent();
            }

            BuildRenderTree(b, ref seq);
        }));
        

        builder.CloseComponent();
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

using HeadlessBlazor.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace HeadlessBlazor;

public class HBDropdown : HBElement<HBDropdown>
{
    public bool IsOpen { get; private set; }

    [Parameter]
    public EventCallback<HBDropdownItem> OnClickItem { get; set; }

    public async Task Toggle() => await SetIsOpen(!IsOpen);
    public async Task Open() => await SetIsOpen(true);
    public async Task Close() => await SetIsOpen(false);

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (!OnClickItem.HasDelegate)
            OnClickItem = new EventCallback<HBDropdownItem>(this, Close);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var seq = 0;
        builder.OpenComponent<CascadingValue<HBDropdown>>(seq++);
        builder.AddAttribute(seq, "Value", this);
        builder.AddAttribute(seq++, "ChildContent", (RenderFragment)((b) => BuildRenderTree(b, ref seq)));
        builder.CloseComponent();
    }

    private Task SetIsOpen(bool isOpen)
    {
        return InvokeAsync(() =>
        {
            IsOpen = isOpen;
            StateHasChanged();
        });
    }
}

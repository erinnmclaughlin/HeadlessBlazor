using HeadlessBlazor.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace HeadlessBlazor;

public class Dropdown : HBElement
{
    public bool IsOpen { get; private set; }

    [Parameter]
    public EventCallback<DropdownItem> OnClickItem { get; set; }

    [Parameter]
    public bool CloseOnOutsideClick { get; set; } = true;

    [Parameter]
    public EventCallback OnClickOutside { get; set; }

    public async Task Toggle() => await SetIsOpen(!IsOpen);
    public async Task Open() => await SetIsOpen(true);
    public async Task Close() => await SetIsOpen(false);

    protected override void OnInitialized()
    {
        if (!OnClickItem.HasDelegate)
            OnClickItem = new EventCallback<DropdownItem>(this, Close);

        if (!OnClickOutside.HasDelegate && CloseOnOutsideClick)
            OnClickOutside = new EventCallback(this, Close);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var seq = 0;
        builder.OpenComponent<CascadingValue<Dropdown>>(seq++);
        builder.AddAttribute(seq, "Value", this);
        builder.AddAttribute(seq++, "ChildContent", (RenderFragment)((b) =>
        {
            if (OnClickOutside.HasDelegate)
            {
                b.OpenElement(seq++, "div");
                b.AddAttribute(seq, "style", "position: fixed; top:0; right: 0; bottom: 0; left:0");
                b.AddAttribute(seq++, "onclick", OnClickOutside);
                b.CloseElement();
            }

            BuildRenderTree(b, ref seq);
        }));
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

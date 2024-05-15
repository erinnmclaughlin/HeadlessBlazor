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
        builder.OpenComponent<CascadingValue<Dropdown>>(CurrentSequence++);
        builder.AddAttribute(CurrentSequence, "Value", this);
        builder.AddAttribute(CurrentSequence++, "ChildContent", (RenderFragment)((b) => {

            // TODO: Figure out how to make this work!
            //if (OnClickOutside.HasDelegate)
            //{
            //    builder.OpenElement(CurrentSequence++, "div");
            //    //builder.AddAttribute(CurrentSequence, "style", "position: fixed; top:0; right: 0; bottom: 0; left:0");
            //    //builder.AddAttribute(CurrentSequence++, "onclick", OnClickOutside);
            //    builder.CloseElement();
            //}

            base.BuildRenderTree(b);
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

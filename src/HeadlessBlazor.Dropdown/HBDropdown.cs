using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace HeadlessBlazor;

/// <summary>
/// The dropdown container.
/// </summary>
public class HBDropdown : HBElement<HBDropdown>
{
    /// <summary>
    /// A reference to the HTML element.
    /// </summary>
    public ElementReference ElementReference { get; private set; }

    /// <summary>
    /// Indicates whether or not the dropdown menu is currently open.
    /// Default is <see langword="false" />.
    /// </summary>
    public bool IsOpen { get; private set; }

    /// <summary>
    /// When <see langword="true"/>, the dropdown menu will close when the escape key is pressed. 
    /// Default is <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool CloseOnEscape { get; set; } = true;

    /// <summary>
    /// When <see langword="true"/>, the dropdown menu will close when a click event occurs outside of the dropdown container. 
    /// Default is <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool CloseOnOutsideClick { get; set; } = true;

    /// <summary>
    /// The default behavior when a dropdown item is clicked.
    /// If a value is not specified, the default click behavior is set to close the dropdown menu.
    /// </summary>
    [Parameter]
    public EventCallback<HBDropdownItem> OnClickItem { get; set; }

    /// <summary>
    /// Invoked when the dropdown menu closes.
    /// </summary>
    [Parameter]
    public EventCallback<HBDropdown> OnClose { get; set; }

    /// <summary>
    /// Invoked when the dropdown menu opens.
    /// </summary>
    [Parameter]
    public EventCallback<HBDropdown> OnOpen { get; set; }

    /// <summary>
    /// Opens the dropdown menu.
    /// </summary>
    public async Task OpenAsync() => await InvokeAsync(() =>
    {
        IsOpen = true;
        StateHasChanged();

        OnOpen.InvokeAsync(this);
    });

    /// <summary>
    /// Closes the dropdown menu.
    /// </summary>
    public async Task CloseAsync() => await InvokeAsync(() =>
    {
        IsOpen = false;
        StateHasChanged();

        OnClose.InvokeAsync(this);
    });

    /// <summary>
    /// Toggles the dropdown menu.
    /// </summary>
    public async Task ToggleAsync()
    {
        await (IsOpen ? CloseAsync() : OpenAsync());
    }

    /// <inheritdoc />
    protected override void OnAfterInitialized()
    {
        if (!OnClickItem.HasDelegate)
            OnClickItem = new EventCallback<HBDropdownItem>(this, CloseAsync);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    protected override void OnBeforeCloseElement(ref int sequence, RenderTreeBuilder builder)
    {
        if (CloseOnEscape)
        {
            builder.AddAttribute(sequence++, "onkeydown", EventCallback.Factory.Create<KeyboardEventArgs>(this, async (args) =>
            {
                if (args.Key == "Escape")
                    await CloseAsync();
            }));
        }

        builder.AddElementReferenceCapture(sequence++, async capturedRef =>
        {
            ElementReference = capturedRef;
            await InvokeAsync(StateHasChanged);
        });

        base.OnBeforeCloseElement(ref sequence, builder);
    }
}

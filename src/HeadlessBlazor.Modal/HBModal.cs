using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace HeadlessBlazor;

/// <summary>
/// The modal container. Owns the open/closed state and cascades itself to the
/// trigger, overlay and content components.
/// </summary>
public class HBModal : HBElement<HBModal>
{
    /// <summary>
    /// Indicates whether or not the modal is currently open.
    /// Default is <see langword="false" />.
    /// </summary>
    public bool IsOpen { get; private set; }

    /// <summary>
    /// When <see langword="true"/>, the modal will close when the escape key is pressed.
    /// Default is <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool CloseOnEscape { get; set; } = true;

    /// <summary>
    /// When <see langword="true"/>, the modal will close when a click event occurs on the
    /// <see cref="HBModalOverlay"/>.
    /// Default is <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool CloseOnOutsideClick { get; set; } = true;

    /// <summary>
    /// Invoked when the modal closes.
    /// </summary>
    [Parameter]
    public EventCallback<HBModal> OnClose { get; set; }

    /// <summary>
    /// Invoked when the modal opens.
    /// </summary>
    [Parameter]
    public EventCallback<HBModal> OnOpen { get; set; }

    /// <summary>
    /// Opens the modal.
    /// </summary>
    public async Task OpenAsync() => await InvokeAsync(() =>
    {
        IsOpen = true;
        StateHasChanged();

        OnOpen.InvokeAsync(this);
    });

    /// <summary>
    /// Closes the modal.
    /// </summary>
    public async Task CloseAsync() => await InvokeAsync(() =>
    {
        IsOpen = false;
        StateHasChanged();

        OnClose.InvokeAsync(this);
    });

    /// <summary>
    /// Toggles the modal.
    /// </summary>
    public async Task ToggleAsync()
    {
        await (IsOpen ? CloseAsync() : OpenAsync());
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var seq = 0;
        builder.OpenComponent<CascadingValue<HBModal>>(seq++);
        builder.AddAttribute(seq++, "Value", this);
        builder.AddAttribute(seq++, "ChildContent", (RenderFragment)(b =>
        {
            BuildRenderTree(ref seq, b);
        }));

        builder.CloseComponent();
    }
}

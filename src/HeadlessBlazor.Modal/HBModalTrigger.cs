using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor;

/// <summary>
/// The modal trigger. Toggles the modal open and closed when clicked.
/// </summary>
public class HBModalTrigger : HBElement
{
    /// <summary>
    /// The parent <see cref="HBModal"/> component.
    /// </summary>
    [CascadingParameter]
    public HBModal? Modal { get; set; }

    /// <inheritdoc />
    [Parameter]
    public override string ElementName { get; set; } = "button";

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        if (Modal is not null && !OnClickPreventDefault)
            UserAttributes.TryAdd("onclick", new EventCallback(this, Modal.ToggleAsync));
    }
}

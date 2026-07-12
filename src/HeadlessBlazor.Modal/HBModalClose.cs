using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor;

/// <summary>
/// A button that closes the modal when clicked.
/// </summary>
public class HBModalClose : HBElement
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
        if (Modal is not null)
        {
            UserAttributes.TryAdd("onclick", new EventCallback(this, Modal.CloseAsync));
        }
    }
}

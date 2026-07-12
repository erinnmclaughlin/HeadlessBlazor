using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor;

/// <summary>
/// A button that dismisses the enclosing modal when clicked (equivalent to calling
/// <see cref="IModalInstance.CancelAsync"/>). To close the modal with a result instead,
/// call <see cref="IModalInstance.CloseAsync"/> directly from the modal body component.
/// </summary>
public class HBModalClose : HBElement
{
    /// <summary>
    /// The enclosing modal.
    /// </summary>
    [CascadingParameter]
    public IModalInstance Modal { get; set; } = null!;

    /// <inheritdoc />
    [Parameter]
    public override string ElementName { get; set; } = "button";

    /// <inheritdoc />
    protected override void OnBeforeInitialized()
    {
        if (Modal is null)
            throw new InvalidOperationException($"{nameof(HBModalClose)} must be used within a modal body rendered by {nameof(HBModalHost)}.");
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        UserAttributes.TryAdd("type", "button");
        UserAttributes.TryAdd("onclick", new EventCallback(this, Modal.CancelAsync));
    }
}

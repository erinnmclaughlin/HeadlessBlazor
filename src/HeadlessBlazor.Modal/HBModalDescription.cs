using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor;

/// <summary>
/// The accessible description of the modal. Registers its <c>id</c> with the enclosing modal's
/// dialog element so the dialog is described via <c>aria-describedby</c>.
/// </summary>
public class HBModalDescription : HBElement
{
    private readonly string _generatedId = $"hb-modal-description-{Guid.NewGuid():N}";

    /// <summary>
    /// The registrar for the enclosing modal's dialog element.
    /// </summary>
    [CascadingParameter]
    protected IModalContentRegistrar? Content { get; set; }

    /// <inheritdoc />
    [Parameter]
    public override string ElementName { get; set; } = "p";

    /// <inheritdoc />
    protected override void OnBeforeInitialized()
    {
        UserAttributes.TryAdd("id", _generatedId);
    }

    /// <inheritdoc />
    protected override void OnAfterInitialized()
    {
        var id = UserAttributes.TryGetValue("id", out var value) && value is string s ? s : _generatedId;
        Content?.RegisterDescribedBy(id);
    }
}

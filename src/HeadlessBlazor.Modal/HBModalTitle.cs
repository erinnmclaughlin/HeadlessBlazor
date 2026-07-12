using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor;

/// <summary>
/// The accessible title of the modal. Registers its <c>id</c> with the parent
/// <see cref="HBModalContent"/> so the dialog is labelled via <c>aria-labelledby</c>.
/// </summary>
public class HBModalTitle : HBElement
{
    private readonly string _generatedId = $"hb-modal-title-{Guid.NewGuid():N}";

    /// <summary>
    /// The parent <see cref="HBModalContent"/> component.
    /// </summary>
    [CascadingParameter]
    protected HBModalContent? Content { get; set; }

    /// <inheritdoc />
    [Parameter]
    public override string ElementName { get; set; } = "h2";

    /// <inheritdoc />
    protected override void OnBeforeInitialized()
    {
        UserAttributes.TryAdd("id", _generatedId);
    }

    /// <inheritdoc />
    protected override void OnAfterInitialized()
    {
        var id = UserAttributes.TryGetValue("id", out var value) && value is string s ? s : _generatedId;
        Content?.RegisterLabelledBy(id);
    }
}

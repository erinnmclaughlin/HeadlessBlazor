namespace HeadlessBlazor;

/// <summary>
/// Cascaded alongside <see cref="IModalInstance"/> so <see cref="HBModalTitle"/> and
/// <see cref="HBModalDescription"/> can register their element ids for <c>aria-labelledby</c>
/// / <c>aria-describedby</c> on the host-rendered dialog element.
/// </summary>
public interface IModalContentRegistrar
{
    /// <summary>
    /// Registers the id of the element that labels the dialog. Called by <see cref="HBModalTitle"/>.
    /// </summary>
    void RegisterLabelledBy(string id);

    /// <summary>
    /// Registers the id of the element that describes the dialog. Called by <see cref="HBModalDescription"/>.
    /// </summary>
    void RegisterDescribedBy(string id);
}

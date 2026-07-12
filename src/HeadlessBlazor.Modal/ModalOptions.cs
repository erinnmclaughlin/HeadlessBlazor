namespace HeadlessBlazor;

/// <summary>
/// Behavioral options for a modal shown via <see cref="IModalService"/>. Headless: there is
/// no styling here, only behavior and attribute passthrough for the host-rendered overlay
/// and dialog elements.
/// </summary>
public class ModalOptions
{
    /// <summary>
    /// When <see langword="true"/>, the modal is cancelled when the escape key is pressed.
    /// Default is <see langword="true"/>.
    /// </summary>
    public bool CloseOnEscape { get; set; } = true;

    /// <summary>
    /// When <see langword="true"/>, the modal is cancelled when the overlay (backdrop) is clicked.
    /// Default is <see langword="true"/>.
    /// </summary>
    public bool CloseOnOutsideClick { get; set; } = true;

    /// <summary>
    /// Additional attributes (e.g. <c>class</c>, <c>style</c>) applied to the overlay element
    /// rendered by <see cref="HBModalHost"/>.
    /// </summary>
    public IDictionary<string, object?>? OverlayAttributes { get; set; }

    /// <summary>
    /// Additional attributes (e.g. <c>class</c>, <c>style</c>) applied to the dialog content
    /// element rendered by <see cref="HBModalHost"/>.
    /// </summary>
    public IDictionary<string, object?>? ContentAttributes { get; set; }
}

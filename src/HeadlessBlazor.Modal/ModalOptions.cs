namespace HeadlessBlazor;

/// <summary>
/// Behavioral options for a modal shown via <see cref="IModalService"/>. Headless: there is
/// no styling here, only behavior and attribute passthrough for the host-rendered overlay
/// and dialog elements.
/// </summary>
public class ModalOptions
{
    /// <summary>
    /// When <see langword="true"/>, the modal is canceled when the escape key is pressed.
    /// Default is <see langword="true"/>.
    /// </summary>
    public bool CloseOnEscape { get; set; } = true;

    /// <summary>
    /// When <see langword="true"/>, the modal is canceled when the overlay (backdrop) is clicked.
    /// Default is <see langword="true"/>.
    /// </summary>
    public bool CloseOnOutsideClick { get; set; } = true;

    /// <summary>
    /// Additional attributes (e.g. <c>class</c>, <c>style</c>) applied to the overlay element
    /// rendered by <see cref="HBModalHost"/>.
    /// </summary>
    public IDictionary<string, object?> OverlayAttributes { get; set; } = new Dictionary<string, object?>();

    /// <summary>
    /// Additional attributes (e.g. <c>class</c>, <c>style</c>) applied to the modal container element.
    /// </summary>
    public IDictionary<string, object?> ContainerAttributes { get; set; } = new Dictionary<string, object?>();

    /// <summary>
    /// Additional attributes (e.g. <c>class</c>, <c>style</c>) applied to the dialog content
    /// element rendered by <see cref="HBModalHost"/>.
    /// </summary>
    public IDictionary<string, object?> ContentAttributes { get; set; } = new Dictionary<string, object?>();

    /// <summary>
    /// When set, enables enter/exit transitions. The overlay and dialog elements are given a
    /// <c>data-state</c> attribute (<c>"closed"</c> then <c>"open"</c> on enter, and back to
    /// <c>"closed"</c> on close) that CSS can transition off, and the modal stays mounted for
    /// this duration after it is closed so the exit transition can play before the element is
    /// removed. Set it to match the duration of your CSS transition. Default is
    /// <see langword="null"/> (no transitions; the modal opens and closes instantly).
    /// </summary>
    public TimeSpan? TransitionDuration { get; set; }
}

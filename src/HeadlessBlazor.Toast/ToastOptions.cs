namespace HeadlessBlazor;

/// <summary>
/// Behavioral options for a toast shown via <see cref="IToastService"/>. Headless: there is
/// no styling here, only behavior and attribute passthrough for the host-rendered toast element.
/// </summary>
public class ToastOptions
{
    /// <summary>
    /// How long the toast stays visible before it is automatically dismissed. Set to
    /// <see langword="null"/> to disable auto-dismiss, requiring the toast to be dismissed
    /// manually (e.g. via <see cref="IToastInstance.DismissAsync"/> or <see cref="HBToastClose"/>).
    /// Default is 5 seconds.
    /// </summary>
    public TimeSpan? Duration { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Additional attributes (e.g. <c>class</c>, <c>style</c>) applied to the toast element
    /// rendered by <see cref="HBToastHost"/>.
    /// </summary>
    public IDictionary<string, object?> Attributes { get; set; } = new Dictionary<string, object?>();

    /// <summary>
    /// When set, enables enter/exit transitions. The toast element is given a <c>data-state</c>
    /// attribute (<c>"closed"</c> then <c>"open"</c> on show, and back to <c>"closed"</c> on
    /// dismiss) that CSS can transition off, and the toast stays mounted for this duration after
    /// it is dismissed so the exit transition can play before the element is removed. Set it to
    /// match the duration of your CSS transition. Default is <see langword="null"/> (no
    /// transitions; the toast appears and disappears instantly).
    /// </summary>
    public TimeSpan? TransitionDuration { get; set; }
}

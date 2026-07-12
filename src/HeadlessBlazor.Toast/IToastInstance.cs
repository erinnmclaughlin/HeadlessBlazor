namespace HeadlessBlazor;

/// <summary>
/// Represents a shown toast. Returned to the caller of <see cref="IToastService.Show{TComponent}(ToastOptions?)"/>
/// and cascaded into the toast's body component (via <see cref="HBToastHost"/>), so either side can
/// dismiss it early - e.g. a caller that wants to replace a toast, or a close button within the body.
/// </summary>
public interface IToastInstance
{
    /// <summary>
    /// A unique identifier for this toast instance.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// The options this toast was shown with.
    /// </summary>
    ToastOptions Options { get; }

    /// <summary>
    /// Dismisses the toast (playing its exit transition first, if enabled). No-op if the toast
    /// has already been dismissed.
    /// </summary>
    Task DismissAsync();
}

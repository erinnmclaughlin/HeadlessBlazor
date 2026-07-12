namespace HeadlessBlazor;

/// <summary>
/// Shows toasts imperatively, from anywhere in the app. Requires an <see cref="HBToastHost"/>
/// to be rendered somewhere in the component tree (typically once, in the app's main layout) -
/// without one, toasts shown via <see cref="Show{TComponent}(ToastOptions?)"/> are never rendered.
/// </summary>
public interface IToastService
{
    /// <summary>
    /// Shows <typeparamref name="TComponent"/> as a toast. Unlike a modal, a toast does not
    /// resolve with a result - it dismisses itself (after <see cref="ToastOptions.Duration"/>
    /// elapses, or via <see cref="IToastInstance.DismissAsync"/>) and is simply removed.
    /// </summary>
    /// <typeparam name="TComponent">The component to render as the toast's body.</typeparam>
    /// <param name="options">Behavioral options for the toast. Defaults to <see cref="ToastOptions"/>'s defaults when omitted.</param>
    /// <returns>A handle to the shown toast that can be used to dismiss it early.</returns>
    IToastInstance Show<TComponent>(ToastOptions? options = null)
        where TComponent : IComponent;

    /// <summary>
    /// Shows <typeparamref name="TComponent"/> as a toast, with parameters bound to it.
    /// </summary>
    /// <typeparam name="TComponent">The component to render as the toast's body.</typeparam>
    /// <param name="parameters">Parameter values to bind to <typeparamref name="TComponent"/>, keyed by parameter name.</param>
    /// <param name="options">Behavioral options for the toast. Defaults to <see cref="ToastOptions"/>'s defaults when omitted.</param>
    /// <returns>A handle to the shown toast that can be used to dismiss it early.</returns>
    IToastInstance Show<TComponent>(IDictionary<string, object?> parameters, ToastOptions? options = null)
        where TComponent : IComponent;
}

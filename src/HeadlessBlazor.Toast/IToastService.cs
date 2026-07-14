namespace HeadlessBlazor;

/// <summary>
/// Shows toasts imperatively, from anywhere in the app. Requires an <see cref="HBToastHost"/>
/// to be rendered somewhere in the component tree (typically once, in the app's main layout) -
/// without one, toasts shown via <see cref="Show{TComponent}(ToastOptions?)"/> are never rendered.
/// </summary>
public interface IToastService
{
    /// <summary>
    /// Begins configuring a toast of <typeparamref name="TComponent"/>, so that its parameters can be
    /// bound fluently and checked by the compiler:
    /// <code>
    /// ToastService.Create&lt;MyToast&gt;()
    ///     .WithParam(x => x.Message, "Saved successfully!")
    ///     .Show();
    /// </code>
    /// The toast is not shown until <see cref="ToastBuilder{TComponent}.Show"/> is called. For
    /// parameters only known at runtime, <see cref="Show{TComponent}(IDictionary{string, object?}, ToastOptions?)"/>
    /// still takes a dictionary directly.
    /// </summary>
    /// <typeparam name="TComponent">The component to render as the toast's body.</typeparam>
    /// <returns>A builder for the toast.</returns>
    ToastBuilder<TComponent> Create<TComponent>()
        where TComponent : IComponent;

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

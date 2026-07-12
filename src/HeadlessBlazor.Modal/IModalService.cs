namespace HeadlessBlazor;

/// <summary>
/// Opens modals imperatively, from anywhere in the app. Requires an <see cref="HBModalHost"/>
/// to be rendered somewhere in the component tree (typically once, in the app's main layout) -
/// without one, <see cref="ShowAsync{TComponent}(ModalOptions?)"/> never resolves.
/// </summary>
public interface IModalService
{
    /// <summary>
    /// Shows <typeparamref name="TComponent"/> as a modal.
    /// </summary>
    /// <typeparam name="TComponent">The component to render as the modal's body.</typeparam>
    /// <param name="options">Behavioral options for the modal. Defaults to <see cref="ModalOptions"/>'s defaults when omitted.</param>
    /// <returns>
    /// A task that completes with the modal's <see cref="ModalResult"/> once it is closed or cancelled.
    /// </returns>
    Task<ModalResult> ShowAsync<TComponent>(ModalOptions? options = null)
        where TComponent : IComponent;

    /// <summary>
    /// Shows <typeparamref name="TComponent"/> as a modal, with parameters bound to it.
    /// </summary>
    /// <typeparam name="TComponent">The component to render as the modal's body.</typeparam>
    /// <param name="parameters">Parameter values to bind to <typeparamref name="TComponent"/>, keyed by parameter name.</param>
    /// <param name="options">Behavioral options for the modal. Defaults to <see cref="ModalOptions"/>'s defaults when omitted.</param>
    /// <returns>
    /// A task that completes with the modal's <see cref="ModalResult"/> once it is closed or cancelled.
    /// </returns>
    Task<ModalResult> ShowAsync<TComponent>(IDictionary<string, object?> parameters, ModalOptions? options = null)
        where TComponent : IComponent;
}

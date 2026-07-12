namespace HeadlessBlazor;

/// <summary>
/// Opens modals imperatively, from anywhere in the app. Requires an <see cref="HBModalHost"/>
/// to be rendered somewhere in the component tree (typically once, in the app's main layout) -
/// without one, <see cref="ShowAsync{TComponent, TResult}(ModalOptions?)"/> never resolves.
/// </summary>
public interface IModalService
{
    /// <summary>
    /// Shows <typeparamref name="TComponent"/> as a modal. The component declares the type of value
    /// it resolves with by implementing <see cref="IModalComponent{TResult}"/>, so
    /// <typeparamref name="TResult"/> is checked at compile time and never cast at runtime.
    /// </summary>
    /// <typeparam name="TComponent">The component to render as the modal's body.</typeparam>
    /// <typeparam name="TResult">The type of value the modal resolves with, as declared by <typeparamref name="TComponent"/>.</typeparam>
    /// <param name="options">Behavioral options for the modal. Defaults to <see cref="ModalOptions"/>'s defaults when omitted.</param>
    /// <returns>
    /// A task that completes with the modal's <see cref="ModalResult{TResult}"/> once it is closed or canceled.
    /// </returns>
    Task<ModalResult<TResult>> ShowAsync<TComponent, TResult>(ModalOptions? options = null)
        where TComponent : IComponent, IModalComponent<TResult>;

    /// <summary>
    /// Shows <typeparamref name="TComponent"/> as a modal, with parameters bound to it.
    /// </summary>
    /// <typeparam name="TComponent">The component to render as the modal's body.</typeparam>
    /// <typeparam name="TResult">The type of value the modal resolves with, as declared by <typeparamref name="TComponent"/>.</typeparam>
    /// <param name="parameters">Parameter values to bind to <typeparamref name="TComponent"/>, keyed by parameter name.</param>
    /// <param name="options">Behavioral options for the modal. Defaults to <see cref="ModalOptions"/>'s defaults when omitted.</param>
    /// <returns>
    /// A task that completes with the modal's <see cref="ModalResult{TResult}"/> once it is closed or canceled.
    /// </returns>
    Task<ModalResult<TResult>> ShowAsync<TComponent, TResult>(IDictionary<string, object?> parameters, ModalOptions? options = null)
        where TComponent : IComponent, IModalComponent<TResult>;
}

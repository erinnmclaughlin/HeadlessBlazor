namespace HeadlessBlazor;

/// <summary>
/// Opens modals imperatively, from anywhere in the app. Requires an <see cref="HBModalHost"/>
/// to be rendered somewhere in the component tree (typically once, in the app's main layout) -
/// without one, <see cref="ShowAsync{TComponent, TResult}(ModalOptions?)"/> never resolves.
/// </summary>
public interface IModalService
{
    /// <summary>
    /// Begins configuring a modal of <typeparamref name="TComponent"/>, so that its parameters can be
    /// bound fluently and checked by the compiler:
    /// <code>
    /// var result = await ModalService.Create&lt;MyModal, bool&gt;()
    ///     .WithParam(x => x.Title, "My Title")
    ///     .WithParam(x => x.Body, "My Body Content")
    ///     .ShowAsync();
    /// </code>
    /// The modal is not shown until <see cref="ModalBuilder{TComponent, TResult}.ShowAsync"/> is called.
    /// For parameters only known at runtime, <see cref="ShowAsync{TComponent, TResult}(IDictionary{string, object?}, ModalOptions?)"/>
    /// still takes a dictionary directly.
    /// </summary>
    /// <typeparam name="TComponent">The component to render as the modal's body.</typeparam>
    /// <typeparam name="TResult">The type of value the modal resolves with, as declared by <typeparamref name="TComponent"/>.</typeparam>
    /// <returns>A builder for the modal.</returns>
    ModalBuilder<TComponent, TResult> Create<TComponent, TResult>()
        where TComponent : IModalComponent<TResult>;

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
        where TComponent : IModalComponent<TResult>;

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
        where TComponent : IModalComponent<TResult>;

    /// <summary>
    /// Shows <typeparamref name="TComponent"/> as a modal, configured through a builder, without
    /// splitting the call in two:
    /// <code>
    /// var result = await ModalService.ShowAsync&lt;MyModal, bool&gt;(b => b
    ///     .WithParam(x => x.Title, "My Title")
    ///     .WithParam(x => x.Body, "My Body Content"));
    /// </code>
    /// Equivalent to <see cref="Create{TComponent, TResult}"/> followed by
    /// <see cref="ModalBuilder{TComponent, TResult}.ShowAsync(ModalOptions?)"/>, for callers who would
    /// rather pass the configuration in than chain onto the result.
    /// </summary>
    /// <typeparam name="TComponent">The component to render as the modal's body.</typeparam>
    /// <typeparam name="TResult">The type of value the modal resolves with, as declared by <typeparamref name="TComponent"/>.</typeparam>
    /// <param name="configure">
    /// Binds the modal's parameters. The builder it receives is a fresh one, so every parameter the
    /// modal needs must be bound here.
    /// </param>
    /// <param name="options">Behavioral options for the modal. Defaults to <see cref="ModalOptions"/>'s defaults when omitted.</param>
    /// <returns>
    /// A task that completes with the modal's <see cref="ModalResult{TResult}"/> once it is closed or canceled.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="configure"/> is <see langword="null"/>.</exception>
    Task<ModalResult<TResult>> ShowAsync<TComponent, TResult>(
        Action<ModalBuilder<TComponent, TResult>> configure, ModalOptions? options = null)
        where TComponent : IModalComponent<TResult>
    {
        ArgumentNullException.ThrowIfNull(configure);

        var builder = Create<TComponent, TResult>();
        configure.Invoke(builder);
        return builder.ShowAsync(options);
    }
}

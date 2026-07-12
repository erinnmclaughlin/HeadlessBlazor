namespace HeadlessBlazor;

/// <summary>
/// Base class for a component used as a modal body. Inheriting it removes the boilerplate of
/// declaring the <see cref="IModalComponent{TResult}"/> contract and cascading in the
/// <see cref="IModalInstance{TResult}"/> by hand: it satisfies the contract itself (so
/// <see cref="IModalService.ShowAsync{TComponent, TResult}(ModalOptions?)"/> can still infer and
/// enforce <typeparamref name="TResult"/> at compile time), and exposes <see cref="CloseAsync"/> /
/// <see cref="CancelAsync"/> as protected members the body can call directly. Because a single
/// <typeparamref name="TResult"/> ties the contract and the cascade together, the caller's result
/// type and the body's can never drift apart.
/// </summary>
/// <typeparam name="TResult">The type of value produced when the modal is closed with a result.</typeparam>
public abstract class HBModal<TResult> : ComponentBase, IModalComponent<TResult>
{
    /// <summary>
    /// The enclosing modal, cascaded by <see cref="HBModalHost"/>.
    /// </summary>
    [CascadingParameter]
    protected IModalInstance<TResult> Instance { get; set; } = null!;

    private IModalInstance<TResult> Modal => Instance
        ?? throw new InvalidOperationException(
            $"{GetType().Name} must be shown via {nameof(IModalService)}.{nameof(IModalService.ShowAsync)} and rendered by {nameof(HBModalHost)}.");

    /// <summary>
    /// Closes the modal, resolving the <see cref="IModalService.ShowAsync{TComponent, TResult}(ModalOptions?)"/>
    /// call that opened it with <paramref name="result"/>.
    /// </summary>
    /// <param name="result">The value to resolve the modal's result with.</param>
    public Task CloseAsync(TResult result) => Modal.CloseAsync(result);

    /// <summary>
    /// Cancels the modal, resolving the <see cref="IModalService.ShowAsync{TComponent, TResult}(ModalOptions?)"/>
    /// call that opened it with a canceled result (see <see cref="ModalResult{TResult}.Canceled"/>).
    /// </summary>
    public Task CancelAsync() => Modal.CancelAsync();
}

/// <summary>
/// A modal that produces a <see cref="bool"/> result.
/// </summary>
public abstract class HBModal : HBModal<bool>
{
    /// <summary>
    /// Closes the modal, resolving the <see cref="IModalService.ShowAsync{TComponent, TResult}(ModalOptions?)"/>
    /// call that opened it with <see langword="true"/>.
    /// </summary>
    /// <returns></returns>
    public Task CloseAsync() => CloseAsync(true);
}
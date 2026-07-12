namespace HeadlessBlazor;

/// <summary>
/// Represents an open modal. Cascaded to the component rendered as the modal's body so it can
/// cancel itself, regardless of the body's result type. Bodies that produce a result close
/// themselves through the strongly-typed <see cref="IModalInstance{TResult}"/> instead.
/// </summary>
public interface IModalInstance
{
    /// <summary>
    /// A unique identifier for this modal instance.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// The options this modal was shown with.
    /// </summary>
    ModalOptions Options { get; }

    /// <summary>
    /// Cancels the modal, resolving the <see cref="IModalService.ShowAsync{TComponent, TResult}(ModalOptions?)"/>
    /// call that opened it with a canceled result (see <see cref="ModalResult{TResult}.Canceled"/>).
    /// </summary>
    Task CancelAsync();
}

/// <summary>
/// Represents an open modal whose body produces a result of type <typeparamref name="TResult"/>.
/// Cascaded to the body component (which declares this result type via
/// <see cref="IModalComponent{TResult}"/>) so it can close itself with a strongly-typed value.
/// </summary>
/// <typeparam name="TResult">The type of value the modal resolves with when closed.</typeparam>
public interface IModalInstance<in TResult> : IModalInstance
{
    /// <summary>
    /// Closes the modal, resolving the <see cref="IModalService.ShowAsync{TComponent, TResult}(ModalOptions?)"/>
    /// call that opened it with <paramref name="result"/>.
    /// </summary>
    /// <param name="result">The value to resolve the modal's result with.</param>
    Task CloseAsync(TResult result);
}

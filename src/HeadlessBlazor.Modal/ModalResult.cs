namespace HeadlessBlazor;

/// <summary>
/// The outcome of a modal shown via <see cref="IModalService.ShowAsync{TComponent, TResult}(ModalOptions?)"/>.
/// </summary>
/// <typeparam name="TResult">The type of the modal's result value.</typeparam>
public readonly struct ModalResult<TResult>
{
    /// <summary>
    /// Indicates whether the modal was dismissed without producing a result (e.g. via
    /// <see cref="IModalInstance.CancelAsync"/>, the escape key, or an outside click) rather than
    /// closed with <see cref="IModalInstance{TResult}.CloseAsync"/>. Kept distinct from
    /// <c>Data is null</c> so a "Cancel" button can be told apart from an explicit <c>CloseAsync(null)</c>.
    /// </summary>
    public bool Canceled { get; }

    /// <summary>
    /// The value passed to <see cref="IModalInstance{TResult}.CloseAsync"/>. Only meaningful when
    /// <see cref="Canceled"/> is <see langword="false"/>; otherwise <see langword="default"/>.
    /// </summary>
    public TResult? Data { get; }

    private ModalResult(bool canceled, TResult? data)
    {
        Canceled = canceled;
        Data = data;
    }

    /// <summary>
    /// Creates a result representing a modal that was closed with <paramref name="data"/>.
    /// </summary>
    public static ModalResult<TResult> Ok(TResult data) => new(false, data);

    /// <summary>
    /// Creates a result representing a modal that was dismissed without a value.
    /// </summary>
    public static ModalResult<TResult> Cancel() => new(true, default);
}

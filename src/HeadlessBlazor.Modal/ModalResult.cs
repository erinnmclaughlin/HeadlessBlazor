namespace HeadlessBlazor;

/// <summary>
/// The outcome of a modal shown via <see cref="IModalService"/>.
/// </summary>
public sealed class ModalResult
{
    /// <summary>
    /// Indicates whether the modal was dismissed without producing a result (e.g. via
    /// <see cref="IModalInstance.CancelAsync"/>, the escape key, or an outside click) rather
    /// than closed with <see cref="IModalInstance.CloseAsync"/>. Kept distinct from
    /// <c>Data is null</c> so a "Cancel" button can be told apart from an explicit
    /// <c>Modal.CloseAsync(false)</c>.
    /// </summary>
    public bool Cancelled { get; }

    /// <summary>
    /// The value passed to <see cref="IModalInstance.CloseAsync"/>, if any.
    /// </summary>
    public object? Data { get; }

    private ModalResult(bool cancelled, object? data)
    {
        Cancelled = cancelled;
        Data = data;
    }

    /// <summary>
    /// Creates a result representing a modal that was closed with an optional value.
    /// </summary>
    public static ModalResult Ok(object? data = null) => new(false, data);

    /// <summary>
    /// Creates a result representing a modal that was dismissed without a value.
    /// </summary>
    public static ModalResult Cancel() => new(true, null);
}

/// <summary>
/// The strongly-typed outcome of a modal shown via <see cref="ModalServiceExtensions"/>.
/// </summary>
/// <typeparam name="TResult">The type of the modal's result value.</typeparam>
public readonly struct ModalResult<TResult>
{
    /// <inheritdoc cref="ModalResult.Cancelled" />
    public bool Cancelled { get; }

    /// <inheritdoc cref="ModalResult.Data" />
    public TResult? Data { get; }

    internal ModalResult(bool cancelled, TResult? data)
    {
        Cancelled = cancelled;
        Data = data;
    }

    /// <inheritdoc cref="ModalResult.Ok" />
    public static ModalResult<TResult> Ok(TResult data) => new(false, data);

    /// <inheritdoc cref="ModalResult.Cancel" />
    public static ModalResult<TResult> Cancel() => new(true, default);
}

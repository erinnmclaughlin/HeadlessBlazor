namespace HeadlessBlazor;

/// <summary>
/// Tracks a single open modal: what to render, its options, its accessibility linkage, and
/// the pending result to hand back to whoever called <see cref="IModalService.ShowAsync{TComponent}(ModalOptions?)"/>.
/// Constructed exclusively by <see cref="ModalService"/> and rendered by <see cref="HBModalHost"/> -
/// not meant to be created directly.
/// </summary>
public sealed class ModalInstance : IModalInstance, IModalContentRegistrar
{
    private readonly ModalService _service;
    private readonly TaskCompletionSource<ModalResult> _resultSource = new(TaskCreationOptions.RunContinuationsAsynchronously);

    internal ModalInstance(ModalService service, Type componentType, IDictionary<string, object?> parameters, ModalOptions options)
    {
        _service = service;
        ComponentType = componentType;
        Parameters = parameters;
        Options = options;
    }

    /// <inheritdoc />
    public Guid Id { get; } = Guid.NewGuid();

    /// <inheritdoc />
    public ModalOptions Options { get; }

    /// <summary>
    /// Whether enter/exit transitions are enabled for this modal (i.e. a positive
    /// <see cref="ModalOptions.TransitionDuration"/> was supplied).
    /// </summary>
    internal bool TransitionsEnabled => Options.TransitionDuration is { } duration && duration > TimeSpan.Zero;

    /// <summary>
    /// The current transition phase, used to drive the rendered <c>data-state</c> attribute.
    /// </summary>
    internal ModalPhase Phase { get; private set; } = ModalPhase.Entering;

    /// <summary>
    /// The value rendered as the <c>data-state</c> attribute on the overlay and dialog elements:
    /// <c>"open"</c> once entered, otherwise <c>"closed"</c> (both while entering and while leaving).
    /// </summary>
    internal string DataState => Phase == ModalPhase.Entered ? "open" : "closed";

    /// <summary>
    /// Advances from <see cref="ModalPhase.Entering"/> to <see cref="ModalPhase.Entered"/> so the
    /// enter transition plays. No-op once the modal has left <see cref="ModalPhase.Entering"/>.
    /// </summary>
    internal void MarkEntered()
    {
        if (Phase == ModalPhase.Entering)
            Phase = ModalPhase.Entered;
    }

    /// <summary>
    /// Moves the modal into <see cref="ModalPhase.Leaving"/> so the exit transition plays.
    /// </summary>
    internal void MarkLeaving() => Phase = ModalPhase.Leaving;

    /// <summary>
    /// The type of the component rendered as the modal's body.
    /// </summary>
    public Type ComponentType { get; }

    /// <summary>
    /// Parameter values bound to the <see cref="ComponentType"/> instance.
    /// </summary>
    public IDictionary<string, object?> Parameters { get; }

    /// <summary>
    /// The <c>id</c> registered by an <see cref="HBModalTitle"/> within this modal, if any.
    /// </summary>
    public string? LabelledBy { get; private set; }

    /// <summary>
    /// The <c>id</c> registered by an <see cref="HBModalDescription"/> within this modal, if any.
    /// </summary>
    public string? DescribedBy { get; private set; }

    internal Task<ModalResult> Result => _resultSource.Task;

    /// <inheritdoc />
    public Task CloseAsync(object? result = null) => _service.CloseAsync(this, ModalResult.Ok(result));

    /// <inheritdoc />
    public Task CancelAsync() => _service.CloseAsync(this, ModalResult.Cancel());

    void IModalContentRegistrar.RegisterLabelledBy(string id)
    {
        LabelledBy = id;
        _service.NotifyStateChanged();
    }

    void IModalContentRegistrar.RegisterDescribedBy(string id)
    {
        DescribedBy = id;
        _service.NotifyStateChanged();
    }

    internal void SetResult(ModalResult result) => _resultSource.TrySetResult(result);
}

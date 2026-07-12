using Microsoft.AspNetCore.Components.Rendering;

namespace HeadlessBlazor;

/// <summary>
/// Tracks a single open modal: what to render, its options, its accessibility linkage, and its
/// transition phase. The result value it hands back to the caller is typed, so this base is
/// abstract and the concrete instance is <see cref="ModalInstance{TResult}"/>. Constructed
/// exclusively by <see cref="HeadlessBlazor.ModalService"/> and rendered by <see cref="HBModalHost"/> - not
/// meant to be created directly.
/// </summary>
public abstract class ModalInstance : IModalInstance, IModalContentRegistrar
{
    private protected readonly ModalService ModalService;

    private protected ModalInstance(ModalService modalService, Type componentType, IDictionary<string, object?> parameters, ModalOptions options)
    {
        ModalService = modalService;
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

    /// <inheritdoc />
    public Task CancelAsync() => ModalService.CloseAsync(this, CompleteCanceled);

    /// <summary>
    /// Completes the pending result with a canceled outcome. Implemented by the typed subclass,
    /// which owns the <see cref="TaskCompletionSource{TResult}"/>.
    /// </summary>
    private protected abstract void CompleteCanceled();

    /// <summary>
    /// Renders the body component wrapped in a cascade of the typed <see cref="IModalInstance{TResult}"/>,
    /// so the body (which knows its own result type) can close itself with a strongly-typed value.
    /// The typed cascade also satisfies non-generic <see cref="IModalInstance"/> consumers such as
    /// <see cref="HBModalClose"/>. Implemented by the typed subclass, which knows <c>TResult</c>.
    /// </summary>
    internal abstract RenderFragment RenderBody();

    void IModalContentRegistrar.RegisterLabelledBy(string id)
    {
        LabelledBy = id;
        ModalService.NotifyStateChanged();
    }

    void IModalContentRegistrar.RegisterDescribedBy(string id)
    {
        DescribedBy = id;
        ModalService.NotifyStateChanged();
    }
}

/// <summary>
/// A concrete open modal whose body resolves with a value of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TResult">The type of the modal's result value.</typeparam>
public sealed class ModalInstance<TResult> : ModalInstance, IModalInstance<TResult>
{
    private readonly TaskCompletionSource<ModalResult<TResult>> _resultSource = new(TaskCreationOptions.RunContinuationsAsynchronously);

    internal ModalInstance(ModalService modalService, Type componentType, IDictionary<string, object?> parameters, ModalOptions options)
        : base(modalService, componentType, parameters, options)
    {
    }

    internal Task<ModalResult<TResult>> Result => _resultSource.Task;

    /// <inheritdoc />
    public Task CloseAsync(TResult result)
        => ModalService.CloseAsync(this, () => _resultSource.TrySetResult(ModalResult<TResult>.Ok(result)));

    private protected override void CompleteCanceled()
        => _resultSource.TrySetResult(ModalResult<TResult>.Cancel());

    internal override RenderFragment RenderBody() => builder =>
    {
        builder.OpenComponent<CascadingValue<IModalInstance<TResult>>>(0);
        builder.AddComponentParameter(1, "Value", this);
        builder.AddComponentParameter(2, "IsFixed", true);
        builder.AddComponentParameter(3, "ChildContent", (RenderFragment)RenderDynamicBody);
        builder.CloseComponent();
    };

    private void RenderDynamicBody(RenderTreeBuilder builder)
    {
        builder.OpenComponent<DynamicComponent>(0);
        builder.AddComponentParameter(1, nameof(DynamicComponent.Type), ComponentType);
        builder.AddComponentParameter(2, nameof(DynamicComponent.Parameters), Parameters);
        builder.CloseComponent();
    }
}

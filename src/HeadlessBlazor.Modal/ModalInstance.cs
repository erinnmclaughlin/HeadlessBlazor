using Microsoft.AspNetCore.Components;

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

namespace HeadlessBlazor;

/// <inheritdoc cref="IModalService" />
internal sealed class ModalService : IModalService
{
    private readonly List<ModalInstance> _instances = [];
    private readonly ModalOptions _defaultOptions;

    public ModalService(ModalOptions defaultOptions)
    {
        _defaultOptions = defaultOptions;
    }

    /// <summary>
    /// Raised whenever a modal is shown, closed, or has its accessibility linkage updated.
    /// Subscribed to by <see cref="HBModalHost"/>.
    /// </summary>
    internal event Action? StateChanged;

    /// <summary>
    /// The currently open modals, oldest first.
    /// </summary>
    internal IReadOnlyList<ModalInstance> Instances => _instances;

    public Task<ModalResult> ShowAsync<TComponent>(ModalOptions? options = null)
        where TComponent : IComponent
        => ShowAsync<TComponent>(new Dictionary<string, object?>(), options);

    public Task<ModalResult> ShowAsync<TComponent>(IDictionary<string, object?> parameters, ModalOptions? options = null)
        where TComponent : IComponent
    {
        var instance = new ModalInstance(this, typeof(TComponent), parameters, options ?? _defaultOptions);
        _instances.Add(instance);
        NotifyStateChanged();
        return instance.Result;
    }

    internal Task CloseAsync(ModalInstance instance, ModalResult result)
    {
        if (_instances.Remove(instance))
            NotifyStateChanged();

        instance.SetResult(result);
        return Task.CompletedTask;
    }

    internal void NotifyStateChanged() => StateChanged?.Invoke();
}

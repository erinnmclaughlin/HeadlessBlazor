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

    internal async Task CloseAsync(ModalInstance instance, ModalResult result)
    {
        // Guard against a double close (e.g. Escape and the close button both firing, or a
        // second close arriving while the exit transition is still playing).
        if (instance.Phase == ModalPhase.Leaving || !_instances.Contains(instance))
            return;

        if (instance.TransitionsEnabled)
        {
            // Keep the instance mounted and flip it back to its closed state so the exit
            // transition can play, then remove it once the configured duration elapses.
            instance.MarkLeaving();
            NotifyStateChanged();
            await Task.Delay(instance.Options.TransitionDuration!.Value);
        }

        if (_instances.Remove(instance))
            NotifyStateChanged();

        instance.SetResult(result);
    }

    internal void NotifyStateChanged() => StateChanged?.Invoke();
}

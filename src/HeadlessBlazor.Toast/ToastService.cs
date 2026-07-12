namespace HeadlessBlazor;

/// <inheritdoc cref="IToastService" />
internal sealed class ToastService : IToastService
{
    private readonly List<ToastInstance> _instances = [];
    private readonly ToastOptions _defaultOptions;

    public ToastService(ToastOptions defaultOptions)
    {
        _defaultOptions = defaultOptions;
    }

    /// <summary>
    /// Raised whenever a toast is shown or dismissed. Subscribed to by <see cref="HBToastHost"/>.
    /// </summary>
    internal event Action? StateChanged;

    /// <summary>
    /// The currently shown toasts, oldest first.
    /// </summary>
    internal IReadOnlyList<ToastInstance> Instances => _instances;

    public IToastInstance Show<TComponent>(ToastOptions? options = null)
        where TComponent : IComponent
        => Show<TComponent>(new Dictionary<string, object?>(), options);

    public IToastInstance Show<TComponent>(IDictionary<string, object?> parameters, ToastOptions? options = null)
        where TComponent : IComponent
    {
        var instance = new ToastInstance(this, typeof(TComponent), parameters, options ?? _defaultOptions);
        _instances.Add(instance);
        NotifyStateChanged();

        if (instance.Options.Duration is { } duration)
            _ = AutoDismissAsync(instance, duration);

        return instance;
    }

    private async Task AutoDismissAsync(ToastInstance instance, TimeSpan duration)
    {
        await Task.Delay(duration);
        await DismissAsync(instance);
    }

    /// <summary>
    /// Removes <paramref name="instance"/> (playing its exit transition first, if enabled).
    /// </summary>
    internal async Task DismissAsync(ToastInstance instance)
    {
        // Guard against a double dismiss (e.g. the auto-dismiss timer and a manual close both
        // firing, or a second dismiss arriving while the exit transition is still playing).
        if (instance.Phase == ToastPhase.Leaving || !_instances.Contains(instance))
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
    }

    internal void NotifyStateChanged() => StateChanged?.Invoke();
}

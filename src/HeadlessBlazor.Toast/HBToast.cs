namespace HeadlessBlazor;

/// <summary>
/// Base class for a component used as a toast body. Inheriting it removes the boilerplate of
/// cascading in <see cref="IToastInstance"/> by hand, and exposes <see cref="DismissAsync"/> as
/// a protected member the body can call directly (e.g. from its own close button).
/// </summary>
public abstract class HBToast : ComponentBase
{
    /// <summary>
    /// The enclosing toast, cascaded by <see cref="HBToastHost"/>.
    /// </summary>
    [CascadingParameter]
    protected IToastInstance Instance { get; set; } = null!;

    private IToastInstance Toast => Instance
        ?? throw new InvalidOperationException(
            $"{GetType().Name} must be shown via {nameof(IToastService)}.{nameof(IToastService.Show)} and rendered by {nameof(HBToastHost)}.");

    /// <summary>
    /// Dismisses the toast (playing its exit transition first, if enabled).
    /// </summary>
    public Task DismissAsync() => Toast.DismissAsync();
}

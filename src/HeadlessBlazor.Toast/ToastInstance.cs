using Microsoft.AspNetCore.Components.Rendering;

namespace HeadlessBlazor;

/// <summary>
/// Tracks a single shown toast: what to render, its options, and its transition phase.
/// Constructed exclusively by <see cref="HeadlessBlazor.ToastService"/> and rendered by
/// <see cref="HBToastHost"/> - not meant to be created directly. Public only because it is used
/// as the parameter type on <see cref="HBToastHostItem"/>.
/// </summary>
public sealed class ToastInstance : IToastInstance
{
    private readonly ToastService _toastService;

    internal ToastInstance(ToastService toastService, Type componentType, IDictionary<string, object?> parameters, ToastOptions options)
    {
        _toastService = toastService;
        ComponentType = componentType;
        Parameters = parameters;
        Options = options;
    }

    /// <inheritdoc />
    public Guid Id { get; } = Guid.NewGuid();

    /// <inheritdoc />
    public ToastOptions Options { get; }

    /// <summary>
    /// The type of the component rendered as the toast's body.
    /// </summary>
    internal Type ComponentType { get; }

    /// <summary>
    /// Parameter values bound to the <see cref="ComponentType"/> instance.
    /// </summary>
    internal IDictionary<string, object?> Parameters { get; }

    /// <summary>
    /// Whether enter/exit transitions are enabled for this toast (i.e. a positive
    /// <see cref="ToastOptions.TransitionDuration"/> was supplied).
    /// </summary>
    internal bool TransitionsEnabled => Options.TransitionDuration is { } duration && duration > TimeSpan.Zero;

    /// <summary>
    /// The current transition phase, used to drive the rendered <c>data-state</c> attribute.
    /// </summary>
    internal ToastPhase Phase { get; private set; } = ToastPhase.Entering;

    /// <summary>
    /// The value rendered as the <c>data-state</c> attribute on the toast element:
    /// <c>"open"</c> once entered, otherwise <c>"closed"</c> (both while entering and while leaving).
    /// </summary>
    internal string DataState => Phase == ToastPhase.Entered ? "open" : "closed";

    /// <summary>
    /// Advances from <see cref="ToastPhase.Entering"/> to <see cref="ToastPhase.Entered"/> so the
    /// enter transition plays. No-op once the toast has left <see cref="ToastPhase.Entering"/>.
    /// </summary>
    internal void MarkEntered()
    {
        if (Phase == ToastPhase.Entering)
            Phase = ToastPhase.Entered;
    }

    /// <summary>
    /// Moves the toast into <see cref="ToastPhase.Leaving"/> so the exit transition plays.
    /// </summary>
    internal void MarkLeaving() => Phase = ToastPhase.Leaving;

    /// <inheritdoc />
    public Task DismissAsync() => _toastService.DismissAsync(this);

    /// <summary>
    /// Renders the body component wrapped in a cascade of <see cref="IToastInstance"/>, so the
    /// body can dismiss itself (e.g. via <see cref="HBToastClose"/> or <see cref="HBToast"/>).
    /// </summary>
    internal RenderFragment RenderBody() => builder =>
    {
        builder.OpenComponent<CascadingValue<IToastInstance>>(0);
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
